// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = '/Soundtracks/AddSinger';
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var actionUrl = '/Soundtracks/AddSinger';
        var dataToSend = { "Name": $("#singerName").get(0).value };//.serialize();
        
        $.post(actionUrl, dataToSend).done(function (data) {
            placeholderElement.find('.modal').modal('hide');
            var singerNewName = data.name;//find('#singerName').get(0).value;
            RefreshSingerList(singerNewName);
        });
    });

    $(document).ready(function () {

        $(document).on('change', ':file', function () {
            var input = $(this),
                localFileName = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselect', [localFileName]);
        });
        

        $(':file').on('fileselect', function (event, localFileName) {
            $('#modalFileUpload').modal('show');

            var input = $(this).parents('.input-group').find(':text');

            if (input.length) {
                input.val(localFileName);
            }

            var formData = new FormData();
            formData.append('file', $('#fileUploadID')[0].files[0]);

            $.ajax({
                url: "/Soundtracks/UploadFile",
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    $('#modalFileUpload').modal('hide');
                    //alert("File " + localFileName + data + " is successfully uploaded!");
                    if (data) {
                        $('#ServerFilename')[0].value = data;
                    }

                },
                error: function (xhr, status, error) {
                    $('#modalFileUpload').modal('hide');
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });


        });
    });

});

function RefreshSingerList(selName) {
    actUrl = "/Soundtracks/GetSingerList";

    $.ajax({
        data: {},
        type: 'GET',
        cache: false,
        dataType: 'json',
        url: actUrl,
        success: function (result) {
            $("#SingerId").empty();
            $.each(result, function (i, item) {
                $("#SingerId").append('<option value="' + item.value + '">' +
                    item.text + '</option>');

                if ((selName) && (item.text === selName)) {
                    $("#SingerId").get(0).options[i].selected = true;
                }
            });
        },
        error: function (ex) {
            alert('We have some technical difficulties...');
        }
    });
}


