function reserveSlot(slotID) {
    var numberOfPeople = document.getElementById('numberOfPeople_' + slotID).value;

    fetch('/api/Reservations/ReserveSlot', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            slotID: slotID,
            numberOfPeople: numberOfPeople
        })
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
        })
        .catch(error => {
            console.error('Error reserving slot:', error);
            alert('An error occurred. Please try again later.');
        });
}
