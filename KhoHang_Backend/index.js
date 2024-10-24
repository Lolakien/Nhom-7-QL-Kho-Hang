const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const orderRoutes = require('./routes/orderRoutes');

const app = express();
const port = 3001;

app.use(cors());
app.use(bodyParser.json());

app.use('/api', orderRoutes);

app.get('/', (req, res) => {
  res.send('Hello World!');
});

app.listen(port, () => {
  console.log(`Backend is running at http://localhost:${port}`);
});