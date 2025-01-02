export async function handleLogin(username, password) {
    const token = await grecaptcha.execute('6LdlhakqAAAAAC86y9F6E_fcFhQ0yI0y5faDDhIm', {action: 'login'});
    console.log('reCAPTCHA token:', token);
    
    const payload = {
        username,
        password,
        captchaToken: token
    };

    const response = await fetch('/v2/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        throw new Error('Login failed');
    }

    return await response.json();
}

export async function handleRegister(username, password) {
    const payload = {
        username,
        password
    };

    const response = await fetch('/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        throw new Error('Registration failed');
    }

    return await response.json();
}