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
        axios.post(`http://localhost:5001/api/CannotDeliverOrder/${orderId}`, formData)
        .then(async response => {
        });
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
