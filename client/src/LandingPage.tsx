import {useNavigate } from 'react-router-dom';
import './LandingPage.css';
import { observer } from 'mobx-react-lite';
import {useEffect, useState} from "react";
import { jwtDecode } from 'jwt-decode';
import axios from 'axios';
//import axios from 'axios';
//import { ClientFormValues } from '../models/client';

const LandingPage = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false)
  const [user, setUser] = useState("")

  const navigate = useNavigate();
  
  const goToCourier = () => {
    navigate('/Courier'); // COŚ INNEGO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  };

  const goToOfficeWorker = () => {
    navigate('/OfficeWorker'); // COŚ INNEGO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  };
  const [, /*setUser*/] = useState({});

  // function handleCallbackResponse(response: any) {
  //     console.log("Encoded INT ID token: " + response.credential);
  //     var userObject = jwtDecode(response.credential);
  //     console.log(userObject);
  //     if (userObject) {
  //         var subc = userObject.sub;

  //         console.log("Subject (sub): " + subc);
  //         if(subc==undefined){
  //             subc = 'error'
  //         }
  //         // navigate('/login', {state: { sub: subc }});
  //         axios.defaults.headers.common['Authorization'] = `Bearer ${response.credential}`;
  //         navigate('/Courier', {state: { sub: subc }});
          
  //     } else {
  //         console.log("No user object found in the decoded token.");
  //     }
  // }

  // useEffect(() => {
  //     const script = document.createElement('script');
  //     script.src = 'https://accounts.google.com/gsi/client';
  //     script.async = true;
  //     script.onload = () => {
  //         (window as any).google.accounts.id.initialize({
  //             client_id: "904528640879-rch2m0d50pu0fbqbk15mt6evhp31vpfv.apps.googleusercontent.com",
  //             // musi być id !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  //             callback: handleCallbackResponse,
  //         });
  //         (window as any).google.accounts.id.renderButton(document.getElementById("signInDiv"),
  //             { theme: "outline",  text: "Log in with Google" , size: "large" , context: "signin"}
  //         );
  //         (window as any).google.accounts.id.prompt();
  //     };
  //     document.body.appendChild(script);

  //     return () => {
  //         document.body.removeChild(script);
  //     };
  // }, []);

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
    
  return (
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

      {/* <div className="landing-container"> */}
      <div className="center">
        <button className="landing-button" onClick={goToOfficeWorker}>
          Office Worker
        </button>
        <button className="landing-button" onClick={goToCourier}>
          Courier
        </button>
      </div>
    </div>
  );
};

export default observer(LandingPage);
