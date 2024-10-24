const express = require('express');
const orderController = require('../controllers/orderController');

const router = express.Router();

router.get('/orders/:orderId', orderController.getOrderStatus);

module.exports = router;