import { handleLogin } from './src/js/auth.js';
import { showSuccessModal } from './src/js/modal.js';

// Replace with your actual site key
const RECAPTCHA_SITE_KEY = 'YOUR_SITE_KEY';

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    try {
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        const data = await handleLogin(username, password);
        console.log('Login successful:', data);
        showSuccessModal();

    } catch (error) {
        console.error('Login error:', error);
        const loginCard = document.querySelector('.login-card');
        loginCard.classList.add('shake');
        
        let errorMessage = document.querySelector('.error-message');
        if (!errorMessage) {
            errorMessage = document.createElement('div');
            errorMessage.className = 'error-message';
            document.querySelector('.login-form').appendChild(errorMessage);
        }
        
        errorMessage.textContent = 'Invalid username or password';
        errorMessage.classList.add('show');
        
        setTimeout(() => {
            loginCard.classList.remove('shake');
        }, 500);
    }
});

// Add input animation handlers
document.querySelectorAll('.form-group input').forEach(input => {
    input.addEventListener('focus', () => {
        input.parentElement.classList.add('focused');
    });
    
    input.addEventListener('blur', () => {
        if (!input.value) {
            input.parentElement.classList.remove('focused');
        }
    });
});