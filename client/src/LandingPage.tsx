import {useNavigate } from 'react-router-dom';
import './LandingPage.css';
import { observer } from 'mobx-react-lite';
import {useEffect, useState} from "react";
import { jwtDecode } from 'jwt-decode';
//import axios from 'axios';
//import { ClientFormValues } from '../models/client';

const LandingPage = () => {
  const navigate = useNavigate();

    const goToForm = () => {
    navigate('/form'); // COŚ INNEGO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  };
    const [, /*setUser*/] = useState({});
    function handleCallbackResponse(response: any) {
        console.log("Encoded INT ID token: " + response.credential);
        var userObject = jwtDecode(response.credential);
        console.log(userObject);
        if (userObject) {
            var subc = userObject.sub;

            console.log("Subject (sub): " + subc);
            if(subc==undefined){
                subc = 'error'
            }
            // history.push({
            //     pathname: '/login',
            //     state: { sub: subc }
            // })
            navigate('/login', {state: { sub: subc }});
            
        } else {
            console.log("No user object found in the decoded token.");
        }
    }

    useEffect(() => {
        const script = document.createElement('script');
        script.src = 'https://accounts.google.com/gsi/client';
        script.async = true;
        script.onload = () => {
            (window as any).google.accounts.id.initialize({
                client_id: "904528640879-rch2m0d50pu0fbqbk15mt6evhp31vpfv.apps.googleusercontent.com", 
                // musi być id !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                callback: handleCallbackResponse,
            });
            (window as any).google.accounts.id.renderButton(document.getElementById("signInDiv"),
                { theme: "outline",  text: "Log in with Google" , size: "large" , context: "signin"}
            );
            (window as any).google.accounts.id.prompt();
        };
        document.body.appendChild(script);

        return () => {
            document.body.removeChild(script);
        };
    }, []);
    
  return (
    <div className="landing-container">
        
       {/* { clientStore.isLoggedIn ? (<div id="signInDiv"/>) : (<Button onClick={clientStore.logout}>Logout</Button>)}*/}
        <div id="signInDiv"/>
      <h1>CourierHub</h1>
      <button className="landing-button" onClick={goToForm}>
        Load Form
      </button>
    </div>
  );
};

export default observer(LandingPage);
