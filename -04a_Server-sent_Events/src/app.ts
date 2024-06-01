import express, { Express, Request, Response } from "express";

const app: Express = express();
const PORT = 8080;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});

app.get("/synchronize-time", async (req: Request, res: Response) => {
  res.writeHead(200, {
    "Content-Type": "text/event-stream",
    "Cache-Control": "no-cache",
    Connection: "keep-alive",
  });

  setInterval(() => {
    return res.write(`data: ${new Date().toLocaleTimeString()}\n\n`);
  }, 1000);
});
