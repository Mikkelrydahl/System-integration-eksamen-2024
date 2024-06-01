import express, { Express, Request, Response } from "express";

const app: Express = express();
const PORT = 5000;
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get("/", async (req: Request, res: Response) => {
  res.send("Welcome to the payment and invoice webhook integrator");
});

app.post("/webhook/paymentGet", async (req: Request, res: Response) => {
  let data = req.body;
  console.log(`Payment get \n${JSON.stringify(data)}`);

  res.send("Webhook get data recieved");
});

app.post("/webhook/paymentDelete", async (req: Request, res: Response) => {
  let data = req.body;
  console.log(`Payment deleted \n${JSON.stringify(data)}`);

  res.send("Webhook delete data recieved");
});

app.listen(PORT, () => {
  console.log(`Webhook is listening on port ${PORT}`);
});
