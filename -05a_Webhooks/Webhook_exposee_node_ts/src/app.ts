import express, { Express, Request, Response } from "express";
import mongoose from "mongoose";
import dotenv from "dotenv";
import { Payment } from "./models/paymentModel";
import { eventNames } from "process";
import axios from "axios";

dotenv.config();
const dbUrl = process.env.DATABASE_URL as string;
mongoose.connect(dbUrl);

const app: Express = express();
const PORT = 80;
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get("/", async (req: Request, res: Response) => {
  res.send("Welcome to the payment and invoice webhook exposee");
});

app.get("/ping", async (req: Request, res: Response) => {
  const paymentDetails = await Payment.find({
    webhookDetails: {
      $elemMatch: {
        $or: [{ eventName: "deletePayment" }, { eventName: "getPayment" }],
      },
    },
  });

  if (paymentDetails) {
    let webhookUrl: string = "";
    paymentDetails.forEach(async (item) => {
      for (let i = 0; i < item.webhookDetails.length; i++) {
        webhookUrl = item.webhookDetails[i].endpointUrl || "";
        if (webhookUrl != null && webhookUrl.length > 0) {
          try {
            // webhook response
            let result = await axios.post(webhookUrl, item, {
              headers: {
                "Content-Type": "application/json",
              },
            });
          } catch (error) {
            console.error("Error sending webhook:", error);
          }
        }
      }
    });
  }
  console.log(`ping response sent to ${paymentDetails.length} webhooks`);
  res.send(`ping response sent to ${paymentDetails.length} webhooks`);
});

app.post("/payment", async (req: Request, res: Response) => {
  const index = (await Payment.find()).length;
  const paymentDetails = new Payment({
    paymentId: index + 1,
    paymentSender: req.body.paymentSender,
    paymentAmount: req.body.paymentAmount,
    paymentCompleted: req.body.paymentCompleted,
  });
  let paymentData = await paymentDetails.save();
  console.log("Payment post for id: " + paymentDetails.paymentId);
  res.send({ result: paymentData });
});

app.get("/payment/:paymentId", async (req: Request, res: Response) => {
  let reqPaymentId = parseInt(req.params.paymentId);
  if (isNaN(reqPaymentId)) {
    res.status(400).send({ error: "Invalid payment id" });
    return;
  }

  const paymentDetails = await Payment.findOne({
    paymentId: reqPaymentId,
  });

  //send webhook when get is called on payment
  if (paymentDetails) {
    let webhookUrl: string = "";
    for (let i = 0; i < paymentDetails.webhookDetails.length; i++) {
      if (paymentDetails.webhookDetails[i].eventName == "getPayment")
        webhookUrl = paymentDetails.webhookDetails[i].endpointUrl || "";
    }
    if (webhookUrl != null && webhookUrl.length > 0) {
      try {
        // webhook response
        let result = await axios.post(webhookUrl, paymentDetails, {
          headers: {
            "Content-Type": "application/json",
          },
        });
        console.log("webhook data sent");
      } catch (error) {
        console.error("Error sending webhook:", error);
      }
    }
  }

  console.log("Payment get for id: " + paymentDetails?.paymentId);
  res.send({ result: paymentDetails });
});

app.delete("/payment/:paymentId", async (req: Request, res: Response) => {
  let reqPaymentId = parseInt(req.params.paymentId);
  if (isNaN(reqPaymentId)) {
    res.status(400).send({ error: "Invalid payment id" });
    return;
  }

  const paymentDetails = await Payment.findOne({
    paymentId: req.params.paymentId,
  });

  if (paymentDetails) {
    let webhookUrl: string = "";
    for (let i = 0; i < paymentDetails.webhookDetails.length; i++) {
      if (paymentDetails.webhookDetails[i].eventName == "deletePayment")
        webhookUrl = paymentDetails.webhookDetails[i].endpointUrl || "";
    }
    if (webhookUrl != null && webhookUrl.length > 0) {
      try {
        // webhook response
        let result = await axios.post(webhookUrl, paymentDetails, {
          headers: {
            "Content-Type": "application/json",
          },
        });
        console.log("webhook data sent");
      } catch (error) {
        console.error("Error sending webhook:", error);
      }
    }
    paymentDetails.deleteOne();
    console.log("Payment delete for id: " + paymentDetails.paymentId);
    res.status(200).send({ result: paymentDetails });
  }
});

app.post("/payment/webhook", async (req: Request, res: Response) => {
  let data = req.body;
  let paymentDetails = await Payment.findOne({ paymentId: data.paymentId });

  if (paymentDetails) {
    //search function to check if webhook already exists
    let search = function (eventname: string, endpointUrl: string) {
      return (
        paymentDetails?.webhookDetails.some((e) =>
          e.eventName?.includes(eventname)
        ) &&
        paymentDetails?.webhookDetails.some((e) =>
          e.endpointUrl?.includes(endpointUrl)
        )
      );
    };

    //check if webhook already exists
    if (search(data.eventName, data.endpointUrl)) {
      res.send({ result: "Webhook already registered" });
      return;
    }

    //add webhook
    paymentDetails.webhookDetails.push({
      eventName: data.eventName,
      endpointUrl: data.endpointUrl,
    });

    paymentDetails = await Payment.findOneAndUpdate(
      {
        paymentId: paymentDetails.paymentId,
      },
      paymentDetails,
      {
        returnOriginal: false,
      }
    );
    console.log("Payment add webhook for id: " + data.paymentId);
    res.send({ result: paymentDetails });
  } else {
    res.send({ result: "Payment not found" });
  }
});

app.delete(
  "/payment/webhook/:paymentId",
  async (req: Request, res: Response) => {
    let reqPaymentId = parseInt(req.params.paymentId);
    if (isNaN(reqPaymentId)) {
      res.status(400).send({ error: "Invalid payment id" });
      return;
    }

    const paymentDetails = await Payment.findOne({
      paymentId: req.params.paymentId,
    });

    if (paymentDetails) {
      for (
        let i = 0;
        i <= (paymentDetails.webhookDetails.length + 1 ?? 0);
        i++
      ) {
        paymentDetails?.webhookDetails.pop();
      }
      paymentDetails?.save();

      console.log(
        "Payment webhook delete for id: " + paymentDetails?.paymentId
      );
      res.status(200).send({ result: paymentDetails });
    }
  }
);

app.listen(PORT, () => {
  console.log(`Webhook is listening on port ${PORT}`);
});

mongoose.connection.on("connected", () => {
  console.log(`Connected to MongoDB ${dbUrl}`);
});
