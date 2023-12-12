//$(document).ready(function () {
//    $('#myTable').DataTable({
//        "scrollY": "1000px",
//        "scrollCollapse": true,
//        "paging": true

//    });
//});

function MoreNewsArticles(count) {
    $.ajax({
        type: "GET",
        url: "/Home/LatestVC",
        data: { count: count },
        success: function (response) {

            $('#latest').html(response);
            document.getElementById("morenews").hidden = true;
        },

        error: function (error) {
            alert("operation failed...", error);
        }

    });
}

    function MoreEditorsArticles(count) {
        $.ajax({
            type: "GET",
            url: "/Home/EditorsVC",
            data: { count: count },
            success: function (response) {

                $('#editors').html(response);
                document.getElementById("morechoice").hidden = true;
            },

            error: function (error) {
                alert("operation failed...", error);
            }

        });
    }