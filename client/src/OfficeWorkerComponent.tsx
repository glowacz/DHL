import { useEffect, useRef, useState } from 'react'
import './App.css'
import axios from 'axios'
import { IOrder } from './models/order'

function OfficeWorkerComponent() {
  const [isLoggedIn, setIsLoggedIn] = useState(false)
  const [user, setUser] = useState("")
  const [orders, setOrders] = useState([])

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

  function getOrders() {
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

    xhr.open('GET', 'https://localhost:5001/api/GetOrdersOfficeWorker');
    xhr.withCredentials = true;
    xhr.send();
  }

  useEffect( () => {
    getOrders();
  }, [])

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

  const handleAccept = (orderId: number) => {
    xhRequest('GET', `https://localhost:5001/api/AcceptOrder/${orderId}`)
  };

  const handleReject = (orderId: number) => {
    xhRequest('GET', `https://localhost:5001/api/RejectOrder/${orderId}`)
  };

  return (
    <div className='container'>
      <div className='header'>
        <div></div>
        <h1>DHL - Office Worker</h1>
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
                {order.status == 0 ? 
                  (<>
                    <button className="ActionButton Accept" onClick={() => handleAccept(order.id!)}>Accept</button>
                    <button className="ActionButton Reject" onClick={() => handleReject(order.id!)}>Reject</button>
                  </>)
                  : 
                  <button className="ActionButton Rejected" disabled>Rejected</button>
                }
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}

export default OfficeWorkerComponent
