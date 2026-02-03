// ========================================
// SMART JEWELRY - CLIENT SCRIPTS
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    initFormValidation();
    initLoadingStates();
    initAnimations();
});

// Form Validation Enhancement
function initFormValidation() {
    // Real-time validation feedback
    document.querySelectorAll('.auth-form input').forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });

        input.addEventListener('input', function() {
            // Remove error state on input
            this.classList.remove('input-validation-error');
            const errorSpan = this.closest('.form-group')?.querySelector('.field-validation-error');
            if (errorSpan) {
                errorSpan.textContent = '';
            }
        });
    });

    // Password match validation
    const confirmPasswordInput = document.querySelector('input[id*="ConfirmPassword"]');
    if (confirmPasswordInput) {
        confirmPasswordInput.addEventListener('input', function() {
            const passwordInput = document.querySelector('input[id*="Password"]:not([id*="Confirm"])');
            if (passwordInput && this.value && this.value !== passwordInput.value) {
                showFieldError(this, 'Mật khẩu xác nhận không khớp');
            } else {
                clearFieldError(this);
            }
        });
    }
}

function validateField(input) {
    const value = input.value.trim();
    const type = input.type;
    let errorMessage = '';

    // Email validation
    if (type === 'email' && value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            errorMessage = 'Email không hợp lệ';
        }
    }

    // Password validation
    if (type === 'password' && value && !input.id.includes('Confirm')) {
        if (value.length < 6) {
            errorMessage = 'Mật khẩu phải có ít nhất 6 ký tự';
        }
    }

    // Phone validation
    if (type === 'tel' && value) {
        const phoneRegex = /^[0-9\s]{10,15}$/;
        if (!phoneRegex.test(value.replace(/\s/g, ''))) {
            errorMessage = 'Số điện thoại không hợp lệ';
        }
    }

    if (errorMessage) {
        showFieldError(input, errorMessage);
    } else {
        clearFieldError(input);
    }
}

function showFieldError(input, message) {
    input.classList.add('input-validation-error');
    let errorSpan = input.closest('.form-group')?.querySelector('.field-validation-error');
    if (errorSpan) {
        errorSpan.innerHTML = `<i class="fas fa-exclamation-circle"></i> ${message}`;
    }
}

function clearFieldError(input) {
    input.classList.remove('input-validation-error');
    const errorSpan = input.closest('.form-group')?.querySelector('.field-validation-error');
    if (errorSpan) {
        errorSpan.textContent = '';
    }
}

// Loading States
function initLoadingStates() {
    document.querySelectorAll('.auth-form').forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn && !submitBtn.disabled) {
                const originalContent = submitBtn.innerHTML;
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<span class="spinner"></span> Đang xử lý...';
                submitBtn.dataset.originalContent = originalContent;
            }
        });
    });
}

// Reset button state
function resetSubmitButton(form) {
    const submitBtn = form.querySelector('button[type="submit"]');
    if (submitBtn && submitBtn.dataset.originalContent) {
        submitBtn.disabled = false;
        submitBtn.innerHTML = submitBtn.dataset.originalContent;
    }
}

// Animations
function initAnimations() {
    // Add staggered animation to feature items
    document.querySelectorAll('.brand-features li').forEach((item, index) => {
        item.style.animationDelay = `${index * 0.1}s`;
    });
}

// Password Toggle (global function for inline onclick)
function togglePassword(btn) {
    const input = btn.parentElement.querySelector('input');
    const icon = btn.querySelector('i');
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}

// Show toast notification
function showToast(type, message, duration = 5000) {
    const toast = document.createElement('div');
    toast.className = `alert alert-${type}`;
    toast.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        animation: slideIn 0.3s ease;
    `;
    
    const icon = type === 'error' ? 'exclamation-circle' : 
                 type === 'success' ? 'check-circle' : 'info-circle';
    
    toast.innerHTML = `
        <i class="fas fa-${icon}"></i>
        <span>${message}</span>
    `;
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, duration);
}

// Add slide animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);
