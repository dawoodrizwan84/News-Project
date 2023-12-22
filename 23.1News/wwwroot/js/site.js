$(document).ready(function () {
    $('#myTable').DataTable({
        "scrollY": "1000px",
        "scrollCollapse": true,
        "paging": true,
        "order": [[0, "desc"]],
        "searching": false

    });
});


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

var quill = new Quill('#QuillEditor', {
    modules: {
        toolbar: [
            ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
            ['blockquote', 'code-block'],

            [{ 'header': 1 }, { 'header': 2 }],               // custom button values
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
            [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
            [{ 'direction': 'rtl' }],                         // text direction

            [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

            [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
            [{ 'font': [] }],
            [{ 'align': [] }],

            ['clean']
        ]
    },
    placeholder: '',
    theme: 'snow'
});

var form = document.querySelector('#new-articles-form');
form.onsubmit = function () {

    var myEditor = document.querySelector('#QuillEditor');
    var html = myEditor.children[0].innerHTML;

    let input = document.createElement('input');
    input.name = "Content";
    input.value = html;
    input.type = "hidden";
    document.getElementById('new-articles-form').appendChild(input);

};

