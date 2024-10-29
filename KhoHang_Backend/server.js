const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');
const axios = require('axios');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 5000;

app.use(cors());
app.use(bodyParser.json());

// API để gọi AI
app.post('/api/chat', async (req, res) => {
    const userMessage = req.body.message;

    try {
        const response = await axios.post('https://api.lambdalabs.com/v1/chat/completions', {
            messages: [{ role: 'user', content: userMessage }],
            model: 'hermes-3-llama-3.1-405b-fp8',
        }, {
            headers: {
                'Authorization': `Bearer ${process.env.API_KEY}`,
                'Content-Type': 'application/json',
            },
        });

        res.json({ reply: response.data.choices[0].message.content });
    } catch (error) {
        console.error(error);
        res.status(500).send('Error calling AI API');
    }
});

// Bắt đầu server
app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});