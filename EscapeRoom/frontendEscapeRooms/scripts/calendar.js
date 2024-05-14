 // DANE TESTOWE - usunąć po podpięciu bazy
 var availability = {
    "2024-05-01": [{name: "Pokój Tajemnic", status: "available"}],
    "2024-05-04": [{name: "Pokój Tajemnic", status: "reserved"}, {name: "Labirynt Złudzeń", status: "available"}],
    "2024-05-05": [{name: "Labirynt Złudzeń", status: "available"}],
    "2024-06-06": [{name: "Pokój Tajemnic", status: "available"}, {name: "Labirynt Złudzeń", status: "available"}, {name: "Kosmiczna Wyprawa", status: "available"}],
    "2024-05-07": [{name: "Pokój Tajemnic", status: "available"}, {name: "Labirynt Złudzeń", status: "reserved"}],
    "2024-05-10": [{name: "Labirynt Złudzeń", status: "available"}, {name: "Kosmiczna Wyprawa", status: "available"}],
    "2024-06-11": [{name: "Pokój Tajemnic", status: "available"}, {name: "Kosmiczna Wyprawa", status: "available"}],
  };

  var currentDate = new Date();
  var currentMonth = currentDate.getMonth();
  var currentYear = currentDate.getFullYear();

  // Funkcja do generowania kalendarza
  function generateCalendar(month, year) {
    var calendarRow = document.getElementById('calendarRow');
    var monthNames = ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"];
    var daysInMonth = new Date(year, month + 1, 0).getDate(); 
    var firstDayOfMonth = new Date(year, month, 1).getDay(); 

    // Ustawianie nazwy aktualnego miesiąca
    document.getElementById('currentMonth').textContent = monthNames[month] + ' ' + year;

    // Usuwanie poprzednich komórek kalendarza
    calendarRow.innerHTML = '';

    // Ustawianie indeksu początkowego dla pierwszego dnia tygodnia
    var startDayIndex = firstDayOfMonth === 0 ? 6 : firstDayOfMonth - 1;

    // Tworzenie wierszy dla każdego tygodnia
    var weekRow = document.createElement('div');
    weekRow.classList.add('row');

    // Wypełnianie pustymi kafelkami na początku miesiąca
    for (var i = 0; i < startDayIndex; i++) {
      var emptyTile = document.createElement('div');
      emptyTile.classList.add('col', 'day-number');
      weekRow.appendChild(emptyTile);
    }

    // Wypełnianie kafelkami z dniami miesiąca
    for (var i = 1; i <= daysInMonth; i++) {
      var dayTile = document.createElement('div');
      dayTile.classList.add('col', 'day-number');

      // Sprawdzenie dostępności pokoi dla danego dnia, wyświetlanie dostępnych pokoi
      var dateKey = year + '-' + ((month + 1) < 10 ? '0' : '') + (month + 1) + '-' + (i < 10 ? '0' : '') + i;
      if (availability[dateKey]) {
        var availableRooms = availability[dateKey].filter(function(room) {
          return room.status === "available";
        });
        //wyświetlanie jednego pokoju podaje jego nazwę na kalendarzu
        if (availableRooms.length === 1) {
          dayTile.innerHTML = `<span>${i}</span><br><small>${availableRooms[0].name}</small>`;
          dayTile.classList.add('available');
          dayTile.setAttribute('data-date', dateKey);
          dayTile.addEventListener('click', function() {
            window.location.href = 'rezerwacja.html?date=' + this.getAttribute('data-date');
          });
          //jeśli więcej to informuje ile jest dostępnych
        } else if (availableRooms.length > 0) {
          dayTile.innerHTML = `<span>${i}</span><br><small>Dostępne ${availableRooms.length} atrakcje</small>`;
          dayTile.classList.add('available');
          dayTile.setAttribute('data-date', dateKey);
          dayTile.addEventListener('click', function() {
            window.location.href = 'rezerwacja.html?date=' + this.getAttribute('data-date');
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
    //uzupełnianie kalendarza pustymi kafelkami na końcu miesiąca
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

  // poprzedni miesiąc
  document.getElementById('prevMonth').addEventListener('click', function() {
    currentMonth--;
    if (currentMonth < 0) {
      currentMonth = 11;
      currentYear--;
    }
    generateCalendar(currentMonth, currentYear);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
  });

  // następny miesiąc
  document.getElementById('nextMonth').addEventListener('click', function() {
    currentMonth++;
    if (currentMonth > 11) {
      currentMonth = 0;
      currentYear++;
    }
    generateCalendar(currentMonth, currentYear);
    localStorage.setItem('lastVisitedMonth', currentMonth);
    localStorage.setItem('lastVisitedYear', currentYear);
  });

  // Generowanie kalendarza, zapamiętanie ostatniego stanu
  window.onload = function() {
    var lastVisitedMonth = localStorage.getItem('lastVisitedMonth');
    var lastVisitedYear = localStorage.getItem('lastVisitedYear');
    if (lastVisitedMonth !== null && lastVisitedYear !== null) {
      currentMonth = parseInt(lastVisitedMonth);
      currentYear = parseInt(lastVisitedYear);
    }
    generateCalendar(currentMonth, currentYear);
  };
