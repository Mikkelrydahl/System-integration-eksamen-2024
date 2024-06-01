import { WebSocket } from "ws";

const ws = new WebSocket("ws://localhost:8080");
console.log("Connecting to server");

ws.on("open", () => {
  console.log("Connected to server");
  ws.send("Hello from Node.js client");

  ws.on("message", (message) => {
    console.log("Received message: '", message.toString(), "'");
    ws.close();
  });
});
