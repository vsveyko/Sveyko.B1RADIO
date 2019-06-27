// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = '/Soundtracks/AddSinger' //$(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var actionUrl = '/Soundtracks/AddSinger';
        var dataToSend = $('form').serialize();
        debugger;
       // alert(dataToSend);

        $.post(actionUrl, dataToSend).done(function (data) {
            placeholderElement.find('.modal').modal('hide');
            RefreshSingerList();
        });
    });

    function RefreshSingerList() {
        actUrl = "/Soundtracks/GetSingerList";
        alert("here!");
        $.ajax({
            data: {},
            type: 'POST',
            cache: false,
            dataType: 'json',
            url: actUrl,
            success: function (result) {
                $("#dropDownSingerId").empty();
                $.each(result, function (i, item) {
                    $("#dropDownSingerId").append('<option value="' + item.Value + '">' +
                        item.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('We have some technical difficulties...');
            }
        });
    };
});


