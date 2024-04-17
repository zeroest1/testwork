document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('newRequestForm');
    const descriptionInput = document.getElementById('description');
    const dueDateInput = document.getElementById('resolutionDueDate');
    const requestsList = document.getElementById('requestsList');

    form.addEventListener('submit', function(event) {
        event.preventDefault();
        submitRequest(descriptionInput.value, dueDateInput.value);
    });

    function submitRequest(description, dueDate) {
        fetch('/requests', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ description, resolutionDueDate: dueDate })
        })
        .then(response => response.ok ? response.json() : Promise.reject(response))
        .then(() => {
            descriptionInput.value = '';
            dueDateInput.value = '';
            loadrequests();
        })
        .catch(error => console.error('Error submitting Request:', error));
    }

    function loadrequests() {
        fetch('/requests')
        .then(response => response.json())
        .then(requests => {
            requestsList.innerHTML = requests.map(Request => {
                const dueDate = new Date(Request.resolutionDueDate);
                const currentDate = new Date();
                const timeRemaining = dueDate - currentDate;
                const timeLeftDisplay = timeRemaining > 0 
                    ? `${Math.floor(timeRemaining / (1000 * 60 * 60 * 24))} days ${Math.floor((timeRemaining / (1000 * 60 * 60)) % 24)} hours`
                    : "expired";
            
                return `<li class="${Request.isUrgent ? 'urgent' : ''}" id="Request-${Request.id}">
                            ${Request.description} - Due: ${dueDate.toLocaleString()}
                            Submitted: ${new Date(Request.submissionTime).toLocaleString()}
                            Time left: ${timeLeftDisplay}
                            <button onclick="resolveRequest(${Request.id})">Resolve</button>
                        </li>`
            }).join('');
            

        })
        .catch(error => console.error('Error loading requests:', error));
    }

    window.resolveRequest = function(id) {
        fetch(`/requests/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            }
        })
        .then(response => response.ok ? response : Promise.reject(response))
        .then(() => loadrequests())
        .catch(error => console.error('Error resolving Request:', error));
    };

    loadrequests();
});
