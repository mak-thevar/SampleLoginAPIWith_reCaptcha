import { handleRegister } from './auth.js';
import { showSuccessModal } from './modal.js';

document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    try {
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        const data = await handleRegister(username, password);
        console.log('Registration successful:', data);
        showSuccessModal();
        
        // Redirect to login page after successful registration
        setTimeout(() => {
            window.location.href = 'index.html';
        }, 3000);

    } catch (error) {
        console.error('Registration error:', error);
        const loginCard = document.querySelector('.login-card');
        loginCard.classList.add('shake');
        
        let errorMessage = document.querySelector('.error-message');
        if (!errorMessage) {
            errorMessage = document.createElement('div');
            errorMessage.className = 'error-message';
            document.querySelector('.login-form').appendChild(errorMessage);
        }
        
        errorMessage.textContent = 'Registration failed. Please try again.';
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