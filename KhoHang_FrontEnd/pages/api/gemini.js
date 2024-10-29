// pages/api/gemini.js
const { GoogleGenerativeAI } = require("@google/generative-ai");

export default async function handler(req, res) {
  if (req.method === 'POST') {
    const { message } = req.body;

    const genAI = new GoogleGenerativeAI(process.env.GOOGLE_API_KEY);
    const model = genAI.getGenerativeModel({ model: "gemini-1.5-flash" });

    try {
      const result = await model.generateContent(message);
      res.status(200).json({ reply: result.response.text() });
    } catch (error) {
      console.error(error);
      res.status(500).json({ message: 'Error calling AI API' });
    }
  } else {
    res.status(405).json({ message: 'Method not allowed' });
  }
}