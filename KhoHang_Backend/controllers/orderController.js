const { sql, poolPromise } = require('../db');

exports.getOrderStatus = async (req, res) => {
  try {
    const { orderId } = req.params;
    const pool = await poolPromise;
    const result = await pool.request()
      .input('orderId', sql.VarChar, orderId)
      .query('SELECT * FROM PhieuXuat WHERE PhieuXuatID = @orderId');

    if (result.recordset.length > 0) {
      const order = result.recordset[0];
      res.json(order);
    } else {
      res.status(404).json({ message: 'Order not found' });
    }
  } catch (err) {
    res.status(500).json({ message: err.message });
  }
};