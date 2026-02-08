// Vietnam Address Selector
// Uses API from provinces.open-api.vn

const VietnamAddress = {
    apiUrl: 'https://provinces.open-api.vn/api',
    
    // Load all provinces/cities
    async loadProvinces(selectElementId) {
        try {
            const response = await fetch(`${this.apiUrl}/p/`);
            const provinces = await response.json();
            
            const select = document.getElementById(selectElementId);
            if (!select) return;
            
            select.innerHTML = '<option value="">-- Chọn Tỉnh/Thành phố --</option>';
            
            provinces.forEach(province => {
                const option = document.createElement('option');
                option.value = province.code;
                option.textContent = province.name;
                option.setAttribute('data-name', province.name);
                select.appendChild(option);
            });
        } catch (error) {
            console.error('Error loading provinces:', error);
        }
    },
    
    // Load districts by province code
    async loadDistricts(provinceCode, selectElementId) {
        try {
            const response = await fetch(`${this.apiUrl}/p/${provinceCode}?depth=2`);
            const data = await response.json();
            
            const select = document.getElementById(selectElementId);
            if (!select) return;
            
            select.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
            
            if (data.districts) {
                data.districts.forEach(district => {
                    const option = document.createElement('option');
                    option.value = district.code;
                    option.textContent = district.name;
                    option.setAttribute('data-name', district.name);
                    select.appendChild(option);
                });
            }
            
            select.disabled = false;
        } catch (error) {
            console.error('Error loading districts:', error);
        }
    },
    
    // Load wards by district code
    async loadWards(districtCode, selectElementId) {
        try {
            const response = await fetch(`${this.apiUrl}/d/${districtCode}?depth=2`);
            const data = await response.json();
            
            const select = document.getElementById(selectElementId);
            if (!select) return;
            
            select.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
            
            if (data.wards) {
                data.wards.forEach(ward => {
                    const option = document.createElement('option');
                    option.value = ward.code;
                    option.textContent = ward.name;
                    option.setAttribute('data-name', ward.name);
                    select.appendChild(option);
                });
            }
            
            select.disabled = false;
        } catch (error) {
            console.error('Error loading wards:', error);
        }
    },
    
    // Initialize form with cascading dropdowns
    initForm(config) {
        const {
            provinceSelectId,
            districtSelectId,
            wardSelectId,
            provinceNameInputId,
            districtNameInputId,
            wardNameInputId
        } = config;
        
        // Load provinces on init
        this.loadProvinces(provinceSelectId);
        
        // Province change event
        const provinceSelect = document.getElementById(provinceSelectId);
        const districtSelect = document.getElementById(districtSelectId);
        const wardSelect = document.getElementById(wardSelectId);
        
        if (provinceSelect) {
            provinceSelect.addEventListener('change', async (e) => {
                const code = e.target.value;
                const selectedOption = e.target.options[e.target.selectedIndex];
                const name = selectedOption?.text || selectedOption?.getAttribute('data-name') || '';
                
                console.log('Province changed:', { code, name }); // Debug
                
                if (provinceNameInputId) {
                    const input = document.getElementById(provinceNameInputId);
                    if (input) {
                        input.value = name;
                        console.log('Set province name:', name); // Debug
                    }
                }
                
                // Reset district and ward
                if (districtSelect) {
                    districtSelect.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
                    districtSelect.disabled = true;
                }
                if (wardSelect) {
                    wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                    wardSelect.disabled = true;
                }
                
                // Clear district and ward names
                if (districtNameInputId) {
                    const input = document.getElementById(districtNameInputId);
                    if (input) input.value = '';
                }
                if (wardNameInputId) {
                    const input = document.getElementById(wardNameInputId);
                    if (input) input.value = '';
                }
                
                if (code) {
                    await this.loadDistricts(code, districtSelectId);
                }
            });
        }
        
        // District change event
        if (districtSelect) {
            districtSelect.addEventListener('change', async (e) => {
                const code = e.target.value;
                const selectedOption = e.target.options[e.target.selectedIndex];
                const name = selectedOption?.text || selectedOption?.getAttribute('data-name') || '';
                
                console.log('District changed:', { code, name }); // Debug
                
                if (districtNameInputId) {
                    const input = document.getElementById(districtNameInputId);
                    if (input) {
                        input.value = name;
                        console.log('Set district name:', name); // Debug
                    }
                }
                
                // Reset ward
                if (wardSelect) {
                    wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                    wardSelect.disabled = true;
                }
                
                // Clear ward name
                if (wardNameInputId) {
                    const input = document.getElementById(wardNameInputId);
                    if (input) input.value = '';
                }
                
                if (code) {
                    await this.loadWards(code, wardSelectId);
                }
            });
        }
        
        // Ward change event
        if (wardSelect) {
            wardSelect.addEventListener('change', (e) => {
                const selectedOption = e.target.options[e.target.selectedIndex];
                const name = selectedOption?.text || selectedOption?.getAttribute('data-name') || '';
                
                console.log('Ward changed:', { name }); // Debug
                
                if (wardNameInputId) {
                    const input = document.getElementById(wardNameInputId);
                    if (input) {
                        input.value = name;
                        console.log('Set ward name:', name); // Debug
                    }
                }
            });
        }
    }
};
