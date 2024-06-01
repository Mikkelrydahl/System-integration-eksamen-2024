import mongoose from "mongoose";

const paymentSchema = new mongoose.Schema({
  paymentId: Number,
  paymentSender: String,
  paymentAmount: Number,
  paymentCompleted: Boolean,
  webhookDetails: [
    {
      eventName: String,
      endpointUrl: String,
    },
  ],
});

export const Payment = mongoose.model("Payment", paymentSchema); //expose model to the rest of the app and link it to the "payments" collection in the database
