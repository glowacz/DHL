import {Button, Form, Header, Label } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite';
import MyTextInput from './MyTextInput';
import {useLocation} from 'react-router-dom';
import {ErrorMessage, Formik } from 'formik';
import { useStore } from './stores/store';
//import { GoogleLogin } from '@react-oauth/google';

export default observer(function LogInFormComponent() {
    const { userStore } = useStore();

    const location = useLocation();
    const sub = (location.state as { sub: string })?.sub;
    // const history = useHistory();
    
    return (
        <div data-testid="login-page">
        <Formik
            initialValues={{ firstName: '',lastName: '', email: '', error: null }}
            /*onSubmit={(values, { setErrors }) =>
                clientStore.login(values).catch(() => setErrors({ error: 'Invalid email or password' }))}*/
            onSubmit={values => {
                console.log(values);
                console.log(sub);
                const updatedValues = { ...values, sub };
                userStore.login(updatedValues)
                // history.replace('/form');
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            }
        >
            {({ handleSubmit, errors }) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Login to Courier Hub' color="teal" textAlign="center" />
                    <MyTextInput placeholder="FirstName" name='firstName' />
                    <MyTextInput placeholder="LastName" name='lastName' />
                    <MyTextInput placeholder="Email" name='email' />
                    <ErrorMessage name='error' render={() =>
                        <Label style={{ marginBottom: 10 }} basic color='red' content={errors.error} />} />
                    <Button  positive content='Login' type="submit" fluid />
                   
                </Form>
            )}

        </Formik>
        </div>
    )
})

