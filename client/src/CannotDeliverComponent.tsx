import { useEffect, useRef, useState } from 'react'
import './App.css'
import axios from 'axios'
import { IOrder } from './models/order'
import { Form } from 'semantic-ui-react'

interface Props {
    orderId: number
}

interface FormData {
    name: string;
    reason: string;
  }

function CannotDeliverComponent({orderId} : Props) {
  function xhRequest(type: string, endpoint: string){
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
            } else {
                console.error('Błąd pobierania danych:', xhr.status);
                // Obsługa błędów
            }
        }
    };

    xhr.open(type, endpoint);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.withCredentials = true;
    xhr.send(JSON.stringify(formData));
  }

    const [formData, setFormData] = useState<FormData>({
        name: '',
        reason: '',
    });

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    function delay(time: number) {
        return new Promise(resolve => setTimeout(resolve, time));
      }

    const handleSubmit = async (e: React.FormEvent) => {
        console.log(JSON.stringify(formData));
        xhRequest('POST', `https://localhost:5001/api/CannotDeliverOrder/${orderId}`)
        // axios.post(`http://localhost:5001/api/CannotDeliverOrder/${orderId}`, formData)
        // .then(async response => {
        // });
        // e.preventDefault();
    }

    return (
        <div>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="name">Name:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <label htmlFor="reason">Reason for not delivering:</label>
          <textarea
            id="reason"
            name="reason"
            value={formData.reason}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <button type="submit">Submit</button>
        </div>
      </form>
    </div>
    )
}

export default CannotDeliverComponent
