(function () {
    var bar = $('.progress-bar');
    var percent = $('.progress-bar');
    var uploadFormBox = $('#uploadFormBox');
    var uploadProgressBox = $('#uploadProgressBox');
    var uploadResultBox = $('#uploadResultBox')
    var fileUploadInput = $('#fileUploadInput');
    var imgUploadedResult = $('#imgUploadedResult');
    var uploadResultBoxDeleteSection = $('#uploadResultBoxDeleteSection');
    var SRC = $('#SRC');
    var deleteFileName = $('#deleteFileName');

    alert(imageFilingCrudMode);

    ///
    ///
    //defining the settings
    ///
    ///
    var acceptedImageFormats = "image/x-png,image/gif,image/jpeg";
    var acceptedImageFormats2 = "image/*";

    fileUploadInput.attr("accept", acceptedImageFormats);

    //setting the hide/show of progressbar
    //it will be shown just before the upload progress strarts
    uploadProgressBox.hide();

    //edit / delete / Create modes

    if (imageFilingCrudMode == "Create") {
        // in create mode 
        // showing upload button

        uploadFormBox.show();
        uploadResultBox.hide();

        // showing result with delete button after successfull upload
        // updating src field value



    } else if (imageFilingCrudMode == "Edit") {
        // in edit mode
        // showing the previously uploaded result with delete button
        // if user deletes the previously uploaded image, we should warn the user which the image will be permanently deleted, and we have to update the src field in the table

        uploadFormBox.hide();
        uploadResultBox.show();


    } else if (imageFilingCrudMode == "Delete") {
        // in delete mode
        // showing previously uploaded image without delete button

        uploadFormBox.hide();
        uploadResultBox.show();
        uploadResultBoxDeleteSection.hide();
    }
    


    //capturing the event of selecting file by user in browse window
    $(document).ready(function () {
        $('input[type="file"]').change(function () {

            uploadProgressBox.show();


            //.ajaxSubmit submit immediately
            //.AjaxForm submit by clicking the submit button
            $('#form1').ajaxSubmit({
                beforeSend: function () {
                    var percentValue = '0%';
                    bar.width(percentValue);
                    percent.html(percentValue);
                },
                uploadProgress: function (event, position, total, percentComplete) {
                    var percentValue = percentComplete + '%';
                    bar.width(percentValue);
                    percent.html(percentValue);
                },
                success: function (d) {
                    var percentValue = '100%';
                    bar.width(percentValue);
                    percent.html(percentValue);
                    fileUploadInput.val('');
                },
                complete: function (xhr) {
                    uploadFormBox.hide();

                    var obj = jQuery.parseJSON(xhr.responseText);
                    
                    if (obj.status == "Success") {
                        uploadResultBox.show();
                        imgUploadedResult.attr("src", "../../" + obj.uploadedFileDirectory + '/' + obj.uploadedFileName);
                        // adding the result to the file name and directory and etc to the form fields
                        SRC.val(obj.uploadedFileName);
                        deleteFileName.val(obj.uploadedFileName);
                    }
                    else {
                        alert(obj.resultMessage);
                    }
                }
            });
        });
    });


    //deleting form

    $('#form0').ajaxForm({
        beforeSerialize: function () {
            uploadProgressBox.hide();
            
        },
        success: function (d) {

        },
        complete: function (xhr) {

            var obj = jQuery.parseJSON(xhr.responseText);

            if (obj.status == "Success") {
                uploadFormBox.show();
                uploadResultBox.hide();
                SRC.val('');
            }
            else {
                alert("Error has been accoured, " + obj.resultMessage);
            }

        }
    });


})();
