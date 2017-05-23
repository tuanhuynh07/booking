//datepicker
$(function () {
    $("#ARTICLE_CREATEDATE").datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
    });
    $("#MEDIA_CREATEDATE").datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
    });
});
//display image when chosen file
function displayImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#image_large').attr('src', e.target.result);
            $('#image_large').css('display', 'block');
        };
        reader.readAsDataURL(input.files[0]);
    }
}
function displayImageThumb(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#image_thumb').attr('src', e.target.result);
            $('#image_thumb').css('display', 'block');
        };
        reader.readAsDataURL(input.files[0]);
    }
}
//kích hoạt
$('.display').click(function () {
    var chossen = $(this);
    var id = $(this).attr("data");
    var data = new FormData();
    data.append('id', id);
    if (id + "" != "") {
        $.ajax({
            url: "/admin/category/DisplayOnMenu",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    if (data.message == "0") {
                        chossen.html('<i title="Không" class="fa fa-ban" aria-hidden="true" style="cursor: pointer; color:red"></i>');
                    }
                    else {
                        chossen.html('<i title="Có" class="fa fa-check" aria-hidden="true" style="cursor: pointer;"></i>');
                    }
                }
            }
        });
    }
});
$('.display_footer').click(function () {
    var chossen = $(this);
    var id = $(this).attr("data");
    var data = new FormData();
    data.append('id', id);
    if (id + "" != "") {
        $.ajax({
            url: "/admincategory/DisplayOnMenuFooter",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    if (data.message == "0") {
                        chossen.html('<i title="Không" class="fa fa-ban" aria-hidden="true" style="cursor: pointer; color:red"></i>');
                    }
                    else {
                        chossen.html('<i title="Có" class="fa fa-check" aria-hidden="true" style="cursor: pointer;"></i>');
                    }
                }
            }
        });
    }
});
$('.active_new').click(function () {
    var chossen = $(this);
    var id = $(this).attr("data");
    var data = new FormData();
    data.append('id', id);
    if (id + "" != "") {
        $.ajax({
            url: "/adminarticle/Active",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    if (data.message == "0") {
                        chossen.html('<i title="Chưa kích hoạt" class="fa fa-ban" aria-hidden="true" style="cursor: pointer; color:red"></i>');
                    }
                    else {
                        chossen.html('<i title="Kích hoạt" class="fa fa-check" aria-hidden="true" style="cursor: pointer;"></i>');
                    }
                }
            }
        });
    }
});
$('.active_media').click(function () {
    var chossen = $(this);
    var id = $(this).attr("data");
    var data = new FormData();
    data.append('id', id);
    if (id + "" != "") {
        $.ajax({
            url: "/adminmedia/Active",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    if (data.message == "0") {
                        chossen.html('<i title="Chưa kích hoạt" class="fa fa-ban" aria-hidden="true" style="cursor: pointer; color:red"></i>');
                    }
                    else {
                        chossen.html('<i title="Kích hoạt" class="fa fa-check" aria-hidden="true" style="cursor: pointer;"></i>');
                    }
                }
            }
        });
    }
});
//crop
//$(function () {
//    //crop image article
//    $('#ARTICLE_IMAGE_LARGE').change(function () {
//        $('#image').hide();
//        var reader = new FileReader();
//        reader.onload = (function (e) {
//            var image = new Image();
//            image.src = e.target.result;
//            image.onload = function () {
//                $('#image').show();
//                $('#image').attr("src", e.target.result);
//                $('#image').Jcrop({
//                    trueSize: [this.width, this.height],
//                    onChange: SetCoordinates,
//                    onSelect: SetCoordinates
//                });
//            };
//        });
//        reader.readAsDataURL($(this)[0].files[0]);
//    });
//});
//function SetCoordinates(c) {
//    $('#imgX1').val(c.x);
//    $('#imgY1').val(c.y);
//    $('#imgWidth').val(c.w);
//    $('#imgHeight').val(c.h);
   
//    var x1 = $('#imgX1').val();
//    var y1 = $('#imgY1').val();
//    var width = $('#imgWidth').val();
//    var height = $('#imgHeight').val();
//    var canvas = $("#canvas")[0];
//    var context = canvas.getContext('2d');
//    var img = new Image();
//    img.onload = function () {
//        canvas.height = height;
//        canvas.width = width;
//        context.drawImage(img, x1, y1, width, height, 0, 0, width, height);
//        $('#imgCropped').val(canvas.toDataURL());
//        $('.imgW').text("Chiều rộng: " + canvas.width+"px");
//        $('.imgH').text(" | Chiều cao: " + canvas.height + "px");
//        $('.imgW').show();
//        $('.imgH').show();
//    };
//    img.src = $('#image').attr("src");
//};
//end crop
//delete
function deleteImageArticle(id,isThumb) {
    var data = new FormData();
    data.append('id', id);
    data.append('isThumb', isThumb);
    if (confirm('Bạn có muốn xóa ảnh này không?')) {
        $.ajax({
            url: "/adminarticle/deleteImage",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    $("#delete_"+isThumb).hide();
                    $('#image_'+isThumb).attr("src", "");
                }
            }
        });
    }
};
function deleteFileArticle(id, intFile) {
    var data = new FormData();
    data.append('id', id);
    data.append('intFile', intFile);
    if (confirm('Bạn có muốn xóa file này không?')) {
        $.ajax({
            url: "/admin/article/deleteFile",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    $("#deleteFile" + intFile).hide();
                    $("#file" + intFile).hide();
                }
            }
        });
    }
};
function deleteImageMedia(id) {
    var data = new FormData();
    data.append('id', id);
    if (confirm('Bạn có muốn xóa ảnh này không?')) {
        $.ajax({
            url: "/admin/media/deleteImage",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    $("#delete_large").hide();
                    $('#image_large').attr("src", "");
                }
            }
        });
    }
};
function deleteImageCategory(id) {
    var data = new FormData();
    data.append('id', id);
    if (confirm('Bạn có muốn xóa ảnh này không?')) {
        $.ajax({
            url: "/admin/category/deleteImage",
            type: "POST",
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data + "" != "") {
                    $("#delete_large").hide();
                    $('#image_large').attr("src", "");
                }
            }
        });
    }
};