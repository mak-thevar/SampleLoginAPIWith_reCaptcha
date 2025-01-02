export function showSuccessModal() {
    const modal = document.getElementById('successModal');
    modal.classList.add('show');
    
    setTimeout(() => {
        modal.classList.remove('show');
    }, 3000);
}