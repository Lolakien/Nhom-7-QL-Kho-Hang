import { useState } from 'react';
import styles from './TrackOrder.module.scss';

export default function TrackOrder() {
  const [orderId, setOrderId] = useState('');
  const [order, setOrder] = useState(null);
  const [error, setError] = useState('');

  const fetchOrderStatus = async () => {
    try {
      const response = await fetch(`http://localhost:3001/api/orders/${orderId}`);
      if (!response.ok) {
        throw new Error('Order not found');
      }
      const data = await response.json();
      setOrder(data);
      setError('');
    } catch (err) {
      setError(err.message);
      setOrder(null);
    }
  };

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Track Order</h1>
      <input
        type="text"
        value={orderId}
        onChange={(e) => setOrderId(e.target.value)}
        placeholder="Enter Order ID"
        className={styles.input}
      />
      <button onClick={fetchOrderStatus} className={styles.button}>Track Order</button>
      {error && <p className={styles.error}>{error}</p>}
      {order && (
        <div className={styles.orderDetails}>
          <h2>Order Details</h2>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Field</th>
                <th>Value</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Order ID</td>
                <td>{order.PhieuXuatID}</td>
              </tr>
              <tr>
                <td>Customer ID</td>
                <td>{order.KhachHangID}</td>
              </tr>
              <tr>
                <td>Employee ID</td>
                <td>{order.NhanVienID}</td>
              </tr>
              <tr>
                <td>Order Date</td>
                <td>{order.NgayXuat}</td>
              </tr>
              <tr>
                <td>Note</td>
                <td>{order.GhiChu}</td>
              </tr>
              <tr>
                <td>Total Amount</td>
                <td>{order.TongTien}</td>
              </tr>
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}