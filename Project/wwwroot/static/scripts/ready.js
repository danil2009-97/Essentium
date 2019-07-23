function ready(value){
  let initial_data = [];
  let timesheets_for_save = [];

  (function () { // table load
      $('[data-toggle="datepicker"]').datepicker({
          format: 'mm-yyyy',
          autoPick: true,
          date: new Date(),
          autoHide: true
      });
      let date = parseInput();
      updateMonthYear(date['month'], date['year'], value);
  })();

  $.ajax({
      type: 'GET',
      url: 'http://localhost:5000/api/Account/InitialData',
      beforeSend: function(request) {
          request.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("token")}`);
      },
      dataType: "json",
      contentType: "application/json; charset=utf-8",
      success: function (data, textStatus) {
          initial_data = data.timesheets;
          insertData(initial_data, value);
          insertEmail(data.managerEmail);
          insertNotfs(data.notifications);
      }
  });

  (function () { // enabling editing rows

      let state = [];

      $('#tablebody').on('click', 'input[name="edit"]', function(){
          toggleEdit(true);
          let row = $(this).parent().parent().get(0);
          // console.log($(parent).attr('leid')); // if undefined -> error
          let tds = $(row).children();
          //creating a container with save/reset buttons
          let cont = document.createElement('div');
          cont.className = 'srcont';
          let save = document.createElement('input');
          save.className = 'tdbutton';
          save.type = 'button';
          save.value = 'Save';
          save.name = 'save';
          let revert = document.createElement('input');
          revert.className = 'tdbutton';
          revert.type = 'button';
          revert.value = 'Revert';
          revert.name = 'revert';
          cont.appendChild(save);
          cont.appendChild(revert);
          $(this).hide();
          //endcreate
          tds[tds.length - 1].appendChild(cont);
          for (var i = 2; i < tds.length - 1; i++) {
            let text = tds[i].innerText;
            state.push(text);
            //let inp = document.createElement('input');
            //inp.className = 'wedit';
            //inp.type = 'text';
            //inp.value = text;
            //tds[i].innerText = '';
            //tds[i].appendChild(inp);
              if (value == 6) {
                  tds[i].innerHTML = `<input type='text' class='wedit' value='${text}'>`;
              }
              else {
                  if (between(i, 3, 6)) {
                      if (text == 'true') {
                          tds[i].innerHTML = `<input type='checkbox' class='wedit' checked>`;
                      }
                      else {
                          tds[i].innerHTML = `<input type='checkbox' class='wedit'>`;
                      }
                  }
                  else {
                      tds[i].innerHTML = `<input type='text' class='wedit' value='${text}'>`;
                  }
              }
          }
      });

      $('#tablebody').on('click', 'input[name="save"]',function(){
          let tds = pFinder($(this));
          let inputs = $('input.wedit');
          let local_for_save = {};
          for (var i = 0; i < inputs.length; i++) {
              if (inputs[i].type == 'checkbox') {
                  tds[i + 2].innerHTML = $(inputs[i]).is(':checked');
              }
              else {
                  tds[i + 2].innerHTML = $(inputs[i]).val();
              }
          }
          let date = tds[1].innerHTML.split('/');
          console.log(value);
          local_for_save['day'] = date[0];
          local_for_save['month'] = date[1];
          if (value == 6){
            local_for_save['bilable'] = tds[2].innerHTML;
            local_for_save['km'] = tds[3].innerHTML;
            local_for_save['comments'] = tds[4].innerHTML;
          }
          else{
            local_for_save['bilable'] = tds[2].innerHTML;
            local_for_save['sickDay'] = tds[3].innerHTML;
            local_for_save['holiday'] = tds[4].innerHTML;
            local_for_save['timeOff'] = tds[5].innerHTML;
            local_for_save['specialLeave'] = tds[6].innerHTML;
            local_for_save['km'] = tds[7].innerHTML;
            local_for_save['comments'] = tds[8].innerHTML;
          }
          // check if similar have been added
          let found = timesheets_for_save.find(function(element) {
            return (element['day'] == local_for_save['day']);
          });
          let idx = timesheets_for_save.indexOf(found);
          if(idx != -1) { timesheets_for_save[idx] = local_for_save; }
          else { timesheets_for_save.push(local_for_save);  }
          toggleEdit(false);
      });

      $('#tablebody').on('click', 'input[name="revert"]',function(){
          let tds = pFinder($(this));
          for (var i = 2; i < tds.length - 1; i++) {
            tds[i].innerHTML = state[i-2];
          }
          state = [];
          toggleEdit(false);
      });
  })();

  (function(){
    $('.bell').one('mouseenter', function() {
      setTimeout(function(){
        $('#ncounter').remove();
        let nids = [];
        $('.ntfa').each(function(){
          nids.push($(this).attr('nid'));
        });
        checkNotifications(nids);
      }, 2000);
    });
  })();

  //savefunctions
  (function(){
    $('#saveall').click(function(){
      let upd_data = [];
      for (let i = 0; i < timesheets_for_save.length; i++) {
        let obj = timesheets_for_save[i];
        let res = initial_data.filter(function(item){
          if (item.day == obj.day  && item.month == obj.month) { return true; }
        })[0];
        for (let key in obj){
          res[key] = obj[key];
        }
        upd_data.push(res);
      }
      saveAll(upd_data);
    });
  })();

  (function(){
    $('#saveandsendall').click(function(){
      let upd_data = [];
      for (let i = 0; i < timesheets_for_save.length; i++) {
        let obj = timesheets_for_save[i];
        let res = initial_data.filter(function(item){
          if (item.day == obj.day  && item.month == obj.month) { return true; }
        })[0];
        for (let key in obj){
          res[key] = obj[key];
        }
        upd_data.push(res);
      }
      saveAll(upd_data);
      notify();
    });
  })();

  (function(){
    $('#revertall').click(function(){
      insertData(initial_data, value);
      timesheets_for_save = [];
    });
  })();

    function cb(newval) {
        initial_data = newval;
        console.log('incallback', initial_data);
    }
  //(function(){
  //  $('#datepicker').change(function(){
  //    initial_data = getFormValue(value);
  //    console.log('initial_data', initial_data);
  //    insertData(initial_data, value);
  //    timesheets_for_save = [];
  //  });
  //})();

  (function(){
      $('#datepicker').change(function () {
          let my = parseInput();
          getTableData(my['month'], my['year'], value, cb);
          timesheets_for_save = [];
    });
  })();


  (function(){
    $('#logout').click(function(){
      localStorage.clear();
      document.location.href = "login.html";
    });
  })();
}
