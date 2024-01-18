import { useEffect, useRef, useState } from 'react'
import './App.css'
import axios from 'axios'
import { IOrder } from './models/order'
import { useNavigate } from 'react-router-dom'
import CannotDeliverComponent from './CannotDeliverComponent'
import agent from './api/agent'

function CourierComponent() {
  const navigate = useNavigate();
  const [orders, setOrders] = useState([])
  const [cannotDeliver, setCannotDeliver] = useState(false)
  const [cannotDeliverId, setCannotDeliverId] = useState(0)

  function getOrders() {
    console.log("getOrders")
    axios.get('http://localhost:5001/api/GetOrdersCourier/11')
    .then(response => {
      setOrders(response.data); // już bez data ????????????????????????????????????????????????????????????????
      console.log(response.data)
      console.log("request")
    })
  }

  useEffect( () => {
    getOrders();
    const intervalId = setInterval(getOrders, 1000);
    return () => clearInterval(intervalId);
  }, [])

  const handleTake = (orderId: number) => {
    agent.Courier.take(orderId, 11)
    .then(_response => {
      getOrders();
    });
  };

  const handlePickup = (orderId: number) => {
    agent.Courier.pickup(orderId, 11)
    .then(_response => {
      getOrders();
    });
  };

  const handleDeliver = (orderId: number) => {
    agent.Courier.deliver(orderId, 11)
    .then(_response => {
      getOrders();
    });
  };

  const handleCannotDeliver = (orderId: number) => {
    // navigate("/CannotDeliver");
    setCannotDeliver(true);
    setCannotDeliverId(orderId);
  };

  function renderButtons(order: IOrder)
  {
    switch(order.status){
      case 1:
        return <button className="Accept" onClick={() => handleTake(order.id!)}>Take</button>
      case 3:
        return <>
            <button className="Accept" onClick={() => handlePickup(order.id!)}>Pickup</button>
            <button className="Reject" onClick={() => handleCannotDeliver(order.id!)}>Cannot deliver</button>
          </>
          
      case 4:
        return <>
          <button className="Accept" onClick={() => handleDeliver(order.id!)}>Deliver</button>
          <button className="Reject" onClick={() => handleCannotDeliver(order.id!)}>Cannot deliver</button>
        </>
    }
  }

  return (
    cannotDeliver ? <CannotDeliverComponent orderId={cannotDeliverId} /> :
    <div>
      <h1>DHL - Courier</h1>
      <ul>
        {orders.map((order: IOrder) => (
          <li key={order.id}>
            {order.destinationAddress.streetName} {order.weight} g
            <div className="action-buttons">
              {renderButtons(order)}
            </div>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default CourierComponent
