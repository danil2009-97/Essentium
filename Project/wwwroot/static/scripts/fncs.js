function parseInput(){
    let date = $('#datepicker').val();
    let ardate = date.split('-');
    return {
        'month': ardate[0],
        'year': ardate[1]
    };
}

//function updateMonthYear(month, year, columns) {
//    $('tr').not(':first').remove();
//    appendRows(month, year, columns);
//    let upd_init_data = getTableData(month, year);
//    console.log('upd', upd_init_data);
//    return upd_init_data;
//}

//function getFormValue(value){
//    let date = parseInput();
//    return updateMonthYear(date['month'], date['year'], value);
//}

function updateMonthYear(month, year, columns) {
    $('tr').not(':first').remove();
    appendRows(month, year, columns);
}

function appendRows(month, year, columns) {
    let daysofweek = {
        '1': 'Monday',
        '2': 'Tuesday',
        '3': 'Wednesday',
        '4': 'Thurday',
        '5': 'Friday',
        'x': ''
    };
    let table = document.getElementById('tablebody');
    let startdate = new Date(month+'/01/'+year);
    let enddate = new Date(startdate.getFullYear(), startdate.getMonth()+1, 0);
    while(startdate <= enddate){
        let row = document.createElement('tr');
        for (let i = 0; i < columns; i++) {
            let td = document.createElement('td');
            if (i == 1) {
                td.innerText = startdate.getDate() +'/' + (startdate.getMonth() + 1);
            }
            else if (i == 0){
                let day = startdate.getDay()
                td.innerText = daysofweek[(day == 0 || day == 6) ? 'x' : day];
            }
            row.appendChild(td);
        }
        table.appendChild(row);
        let newDate = startdate.setDate(startdate.getDate() + 1);
        startdate = new Date(newDate);
    }
}

function between(x, min, max) {
    return x >= min && x <= max;
}

function insertData(timesheets, value){ // change when done
    let table = document.getElementById('tablebody');
    let rows = table.rows;
    for (let i = 0; i<rows.length; i++){
        let row = rows[i];
        let cells = rows[i].cells;
        let date = cells[1].innerHTML;
        for (let r = 0; r<timesheets.length; r++){
            let jdate = timesheets[r]['day']+'/'+timesheets[r]['month'];
            if (date === jdate){
                row.setAttribute('leid', jdate);
                if (value == 6){
                  cells[2].innerHTML = timesheets[r]['bilable'];
                  cells[3].innerHTML = timesheets[r]['km'];
                  cells[4].innerHTML = timesheets[r]['comments'];
                  cells[5].innerHTML = "<input type='button' class='tdbutton' name='edit' value='Edit'>";
                }
                else {
                  cells[2].innerHTML = timesheets[r]['bilable'];
                  cells[3].innerHTML = timesheets[r]['sickDay'];
                  cells[4].innerHTML = timesheets[r]['holiday'];
                  cells[5].innerHTML = timesheets[r]['timeOff'];
                  cells[6].innerHTML = timesheets[r]['specialLeave'];
                  cells[7].innerHTML = timesheets[r]['km'];
                  cells[8].innerHTML = timesheets[r]['comments'];
                  cells[9].innerHTML = "<input type='button' class='tdbutton' name='edit' value='Edit'>";
                }
            }
        }
    }
}

function insertDataMail(timesheets, value){
    let table = document.getElementById('tablebody');
    let rows = table.rows;
    for (let i = 0; i<rows.length; i++){
        let row = rows[i];
        let cells = rows[i].cells;
        let date = cells[1].innerHTML;
        for (let r = 0; r<timesheets.length; r++){
            let jdate = timesheets[r]['day']+'/'+timesheets[r]['month'];
            if (date === jdate){
                row.setAttribute('leid', jdate);
                if (value == 6){
                  cells[2].innerHTML = timesheets[r]['bilable'];
                  cells[3].innerHTML = timesheets[r]['km'];
                  cells[4].innerHTML = timesheets[r]['comments'];
                }
                else {
                  cells[2].innerHTML = timesheets[r]['bilable'];
                  cells[3].innerHTML = timesheets[r]['sickDay'];
                  cells[4].innerHTML = timesheets[r]['holiday'];
                  cells[5].innerHTML = timesheets[r]['timeOff'];
                  cells[6].innerHTML = timesheets[r]['specialLeave'];
                  cells[7].innerHTML = timesheets[r]['km'];
                  cells[8].innerHTML = timesheets[r]['comments'];
                }
            }
        }
    }
}

function insertEmail(email){
    $('#manem').val(email);
}

function insertNotfs(notifications){
  let cn = $('.dropdown-content')[1];
  let filtered_nfts = notifications.filter(function(item){
    if(item.is_Seen === false) { return true; }
  });
    let nl = filtered_nfts.length;
  if (nl > 0){
    $('#ncounter').text(nl);
    $('#ncounter').css('display', 'block');
    cn.innerHTML = '';
    for (var i = 0; i < nl; i++) {
      if (notifications[i].is_Approved === false){
          //cn.innerHTML += `<a class="napp ntfa" nid="${notifications[i].id}"><span class="dot"></span>&nbsp&nbsp&nbspTimesheet for ${notifications[i].year} has NOT been approved</a>`;
          cn.innerHTML += `<a class="napp ntfa" nid="${notifications[i].id}"><span class="dot"></span>&nbsp&nbsp&nbspTimesheet for ${notifications[i].month}/${notifications[i].year} has NOT been approved</a>`;
      }
      else {
         // cn.innerHTML += `<a class="app ntfa" nid="${notifications[i].id}"><span class="dot"></span>&nbsp&nbsp&nbspTimesheet for ${notifications[i].year} has been approved</a>`;
          cn.innerHTML += `<a class="napp ntfa" nid="${notifications[i].id}"><span class="dot"></span>&nbsp&nbsp&nbspTimesheet for ${notifications[i].month}/${notifications[i].year} has been approved</a>`;
      }
    }
  }
}

function toggleEdit(ynbool){
  $('input[name="edit"]').not(this).each(function(){
    $(this).prop('disabled', ynbool);
  });
}

function pFinder(el){
  let cont = el.parent().get(0);
  $(cont).hide();
  let row = el.parent().parent().parent().get(0);
  let tds = $(row).children();
  let td = tds[tds.length - 1];
  td.innerHTML +=  "<input type='button' class='tdbutton' name='edit' value='Edit'>";
  return tds;
}

// ajaxes
function getTableData(month, year, value, callback){
    //let upd_init_data = [];
    let nm = '';
    if (month.charAt(0) === '0') { nm = month.substr(1); }
    else { nm = month; }
    let mnt = parseInt(nm);
    let yr = parseInt(year);
    console.log(mnt, yr);
    $.ajax({
        type: 'POST',
        url: 'http://localhost:5000/api/Account/TableData',
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
        },
        data: `{
          'year': ${yr},
          'month': ${mnt}
      }`,
        //async: false,
      contentType: "application/json; charset=utf-8",
        success: function (data) {
            let upd_init_data = data.items;
            callback(upd_init_data);
            updateMonthYear(month, year, value);
            insertData(upd_init_data, value);
      },
      error: function (err) {
        alert(`The data cannot be fetched, error code - ${err.status}`);
      }
    });
    //return upd_init_data;
}

function changeEmail(){
  let email = $('#manem').val();
  $.ajax({
      type: 'GET',
      url: `http://localhost:5000/api/Account/ChangeManagerEmail/${email}`,
      beforeSend: function(request) {
          request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
      },
      contentType: "text/plain; charset=utf-8",
      success: function () {
        alert('The email has been changed.');
      },
      error: function(err){
          alert(`The email cannot be changed, error code - ${err.status}`);
      }
  });
}

function checkNotifications(nids){
  for (let i = 0; i < nids.length; i++) {
    $.ajax({
        type: 'GET',
        url: `http://localhost:5000/api/Account/CheckNotification/${nids[i]}`,
        beforeSend: function(request) {
            request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
        },
        contentType: "application/json; charset=utf-8",
    });
  }
}

function saveAll(data) {
    let dt = JSON.stringify(data);
    console.log(dt);
    let jdata = {
        items: data,
        is_Editable: false
    };
    console.log(jdata);
    $.ajax({
        type: 'POST',
        url: 'http://localhost:5000/api/Account/AddOrUpdateTimeSheetRange',
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
        },
        data: JSON.stringify(jdata),
      contentType: "application/json; charset=utf-8",
      success: function(){
        alert('The data has been saved!');
      },
      error: function(err){
          alert(`The data cannot be save, error code - ${err.status}`);
      }
  });
}

function notify(){
  $.ajax({
      type: 'GET',
      url: 'http://localhost:5000/api/Account/SendNotification',
      beforeSend: function(request) {
          request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
      },
      success: function(){
        alert('A notification has been sent!');
      },
      error: function(err){
          alert(`A notification cannot be sent, error code - ${err.status}`);
      }
  });
}
