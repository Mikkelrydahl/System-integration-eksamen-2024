import { WebSocketServer } from "ws";

const PORT = process.env.PORT ?? 8080;
const wss = new WebSocketServer({ port: PORT });
console.log("Server started on port", PORT);

wss.on("connection", (ws) => {
  console.log("Clients connected", wss.clients.size);

  ws.on("message", (message) => {
    console.log("Received message: '", message.toString(), "'");

    wss.clients.forEach((client) => {
      client.send(String(message));
    });
  });

  ws.on("close", () => {
    console.log("Client disconnected", wss.clients.size);
  });
});
