// Test data - remove after connecting to the database
var availability = {
    "2024-05-01": [{ name: "Pokój Tajemnic", status: "available" }],
    "2024-05-04": [{ name: "Pokój Tajemnic", status: "reserved" }, { name: "Labirynt Złudzeń", status: "available" }],
    "2024-05-05": [{ name: "Labirynt Złudzeń", status: "available" }],
    "2024-06-06": [{ name: "Pokój Tajemnic", status: "available" }, { name: "Labirynt Złudzeń", status: "available" }, { name: "Kosmiczna Wyprawa", status: "available" }],
    "2024-05-07": [{ name: "Pokój Tajemnic", status: "available" }, { name: "Labirynt Złudzeń", status: "reserved" }],
    "2024-05-10": [{ name: "Labirynt Złudzeń", status: "available" }, { name: "Kosmiczna Wyprawa", status: "available" }],
    "2024-06-11": [{ name: "Pokój Tajemnic", status: "available" }, { name: "Kosmiczna Wyprawa", status: "available" }],
};

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
                dayTile.innerHTML = `<span>${i}</span><br><small>Dostępne ${availableRooms.length} atrakcje</small>`;
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
}

// Default selection of all rooms
var selectedRooms = Object.keys(availability).map(date => availability[date].map(room => room.name)).flat();
// Generating the calendar
generateCalendar(currentMonth, currentYear, selectedRooms);

// Function to handle room selection change
function handleRoomSelectionChange() {
    var selectedRoomCheckboxes = document.querySelectorAll('input[name="selectedRooms"]:checked');
    selectedRooms = [];
    selectedRoomCheckboxes.forEach(function (checkbox) {
        selectedRooms.push(checkbox.value);
    });
    generateCalendar(currentMonth, currentYear, selectedRooms);
}

// Adding event listeners for room selection changes
var roomCheckboxes = document.querySelectorAll('input[name="selectedRooms"]');
roomCheckboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', handleRoomSelectionChange);
});

// Previous month
document.getElementById('prevMonth').addEventListener('click', function () {
    currentMonth--;
    if (currentMonth < 0) {
        currentMonth = 11;
        currentYear--;
    }
    generateCalendar(currentMonth, currentYear, selectedRooms);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
});

// Next month
document.getElementById('nextMonth').addEventListener('click', function () {
    currentMonth++;
    if (currentMonth > 11) {
        currentMonth = 0;
        currentYear++;
    }
    generateCalendar(currentMonth, currentYear, selectedRooms);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
});

// Generate calendar and remember last state
window.onload = function () {
    var lastVisitedMonth = localStorage.getItem('lastVisitedMonth');
    var lastVisitedYear = localStorage.getItem('lastVisitedYear');
    if (lastVisitedMonth !== null && lastVisitedYear !== null) {
        currentMonth = parseInt(lastVisitedMonth);
        currentYear = parseInt(lastVisitedYear);
    }
    generateCalendar(currentMonth, currentYear, selectedRooms);
};
