import { useEffect, useRef, useState } from 'react'
import './App.css'
import axios from 'axios'
import { IOrder } from './models/order'
import { useNavigate } from 'react-router-dom'
import CannotDeliverComponent from './CannotDeliverComponent'
import agent from './api/agent'

function CourierComponent() {
  const [isLoggedIn, setIsLoggedIn] = useState(false)
  const [user, setUser] = useState("")
  const navigate = useNavigate();
  const [orders, setOrders] = useState([])
  const [cannotDeliver, setCannotDeliver] = useState(false)
  const [cannotDeliverId, setCannotDeliverId] = useState(0)

  function getUser(){
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                // const response = JSON.parse(xhr.responseText);
                const response = xhr.responseText;
                console.log(response);
                setUser(response);
                setIsLoggedIn(true);
            } else {
                console.error('Błąd pobierania danych:', xhr.status);
                // Obsługa błędów
            }
        }
    };

    xhr.open('GET', 'https://localhost:5001/Auth/get-current-user');
    xhr.withCredentials = true;
    xhr.send();
  }

  useEffect(() => {
    getUser();
  }, []);

  function xhRequest(type: string, endpoint: string){
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                getOrders();
            } else {
                console.error('Błąd pobierania danych:', xhr.status);
                // Obsługa błędów
            }
        }
    };

    xhr.open(type, endpoint);
    xhr.withCredentials = true;
    xhr.send();
  }

  function getOrders() {
    console.log("getOrders");

    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                const response = JSON.parse(xhr.responseText);
                console.log(response);
                setOrders(response);
            } else {
                console.error('Błąd pobierania danych:', xhr.status);
                // Obsługa błędów
            }
        }
    };

    xhr.open('GET', 'https://localhost:5001/api/GetOrdersCourier');
    xhr.withCredentials = true;
    xhr.send();
  }

  useEffect( () => {
    /* global google */
    getOrders();
    const intervalId = setInterval(getOrders, 1000);
    return () => clearInterval(intervalId);
  }, [])

  const handleTake = (orderId: number) => {
    xhRequest('GET', `https://localhost:5001/api/TakeOrder/${orderId}`)
  };

  const handlePickup = (orderId: number) => {
    xhRequest('GET', `https://localhost:5001/api/PickupOrder/${orderId}`)
    // agent.Courier.pickup(orderId, 11)
    // .then(_response => {
    //   getOrders();
    // });
  };

  const handleDeliver = (orderId: number) => {
    xhRequest('GET', `https://localhost:5001/api/DeliverOrder/${orderId}`)
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
        return <button className="ActionButton Accept" onClick={() => handleTake(order.id!)}>Take</button>
      case 3:
        return <>
            <button className="ActionButton Accept" onClick={() => handlePickup(order.id!)}>Pickup</button>
            <button className="ActionButton Reject" onClick={() => handleCannotDeliver(order.id!)}>Cannot deliver</button>
          </>
          
      case 4:
        return <>
          <button className="ActionButton Accept" onClick={() => handleDeliver(order.id!)}>Deliver</button>
          <button className="ActionButton Reject" onClick={() => handleCannotDeliver(order.id!)}>Cannot deliver</button>
        </>
    }
  }

  return (
    cannotDeliver ? <CannotDeliverComponent orderId={cannotDeliverId} /> :
    <div className='container'>
      <div className='header'>
        <div></div>
        <h1>DHL</h1>
        <div className='top-right'>
          {!isLoggedIn ? 
          <a className='loginButton' 
          href="https://localhost:5001/Auth/login-google">Zaloguj z google</a>
          : <p className='user-text'>Zalogowano jako {user}</p>
          }
          {isLoggedIn ? 
          (<a className='loginButton' 
          href="https://localhost:5001/Auth/logout-google">Wyloguj z google</a>):null}
        </div>
      </div>

      <div className="center">
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
    </div>
  )
}

export default CourierComponent
