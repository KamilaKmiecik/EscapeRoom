var availability = {};

async function fetchAvailability() {
    try {
        const response = await fetch('/api/Reservations/availability');
        if (response.ok) {
            availability = await response.json();
            console.log("Availability data fetched successfully:", availability); 
            var selectedRooms = Object.keys(availability).map(date => availability[date].map(room => room.name)).flat();
            generateCalendar(currentMonth, currentYear, selectedRooms);
        } else {
            console.error('Failed to fetch availability:', response.statusText);
        }
    } catch (error) {
        console.error('Error fetching availability:', error);
    }
}

var currentDate = new Date();
var currentMonth = currentDate.getMonth();
var currentYear = currentDate.getFullYear();

// Function to generate the calendar
function generateCalendar(month, year, selectedRooms) {
    var calendarRow = document.getElementById('calendarRow');
    var monthNames = ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"];
    var daysInMonth = new Date(year, month + 1, 0).getDate();
    var firstDayOfMonth = new Date(year, month, 1).getDay();

    // Setting the current month's name
    document.getElementById('currentMonth').textContent = monthNames[month] + ' ' + year;

    // Removing previous calendar cells
    calendarRow.innerHTML = '';

    // Setting the start index for the first day of the week
    var startDayIndex = firstDayOfMonth === 0 ? 6 : firstDayOfMonth - 1;

    // Creating rows for each week
    var weekRow = document.createElement('div');
    weekRow.classList.add('row');

    // Filling in empty tiles at the beginning of the month
    for (var i = 0; i < startDayIndex; i++) {
        var emptyTile = document.createElement('div');
        emptyTile.classList.add('col', 'day-number');
        weekRow.appendChild(emptyTile);
    }

    // Filling in tiles with days of the month
    for (var i = 1; i <= daysInMonth; i++) {
        var dayTile = document.createElement('div');
        dayTile.classList.add('col', 'day-number');

        // Checking availability of rooms for the given day, displaying available rooms
        var dateKey = year + '-' + ((month + 1) < 10 ? '0' : '') + (month + 1) + '-' + (i < 10 ? '0' : '') + i;
        if (availability[dateKey]) {
            var availableRooms = availability[dateKey].filter(function (room) {
                return selectedRooms.includes(room.name) && room.status === "available";
            });
            // Displaying available rooms
            if (availableRooms.length === 1) {
                dayTile.innerHTML = `<span>${i}</span><br><small>${availableRooms[0].name}</small>`;
                dayTile.classList.add('available');
                dayTile.setAttribute('data-date', dateKey);
                dayTile.addEventListener('click', function () {
                    window.location.href = `/Rezerwacje/Day?date=${this.getAttribute('data-date')}`;
                });
            } else if (availableRooms.length > 0) {
                dayTile.innerHTML = `<span>${i}</span><br><small>Dostępne ${availableRooms.length} Terminy/ów</small>`;
                dayTile.classList.add('available');
                dayTile.setAttribute('data-date', dateKey);
                dayTile.addEventListener('click', function () {
                    window.location.href = `/Rezerwacje/Day?date=${this.getAttribute('data-date')}`;
                });
            } else {
                dayTile.textContent = i;
            }
        } else {
            dayTile.textContent = i;
        }

        weekRow.appendChild(dayTile);

        if ((startDayIndex + i) % 7 === 0) {
            calendarRow.appendChild(weekRow);
            weekRow = document.createElement('div');
            weekRow.classList.add('row');
        }
    }
    // Filling in empty tiles at the end of the month
    var remainingEmptyTiles = 7 - ((startDayIndex + daysInMonth) % 7);
    if (remainingEmptyTiles !== 7) {
        for (var i = 0; i < remainingEmptyTiles; i++) {
            var emptyTile = document.createElement('div');
            emptyTile.classList.add('col', 'day-number');
            weekRow.appendChild(emptyTile);
        }
        calendarRow.appendChild(weekRow);
    }

    // Adding event listeners for room selection changes
    var roomCheckboxes = document.querySelectorAll('input[name="selectedRooms"]');
    roomCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', handleRoomSelectionChange);
    });
}

// Function to handle room selection change
function handleRoomSelectionChange() {
    var selectedRoomCheckboxes = document.querySelectorAll('input[name="selectedRooms"]:checked');
    var selectedRooms = [];
    selectedRoomCheckboxes.forEach(function (checkbox) {
        selectedRooms.push(checkbox.value);
    });
    generateCalendar(currentMonth, currentYear, selectedRooms);
}

// Previous month
document.getElementById('prevMonth').addEventListener('click', async function () {
    currentMonth--;
    if (currentMonth < 0) {
        currentMonth = 11;
        currentYear--;
    }
    await fetchAvailability();
    generateCalendar(currentMonth, currentYear, selectedRooms);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
});

// Next month
document.getElementById('nextMonth').addEventListener('click', async function () {
    currentMonth++;
    if (currentMonth > 11) {
        currentMonth = 0;
        currentYear++;
    }
    await fetchAvailability();
    generateCalendar(currentMonth, currentYear, selectedRooms);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
});

// Fetching availability and generating the calendar
fetchAvailability();
