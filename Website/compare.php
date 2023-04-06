<!DOCTYPE html>
<html lang="en">
<head>

    <title>eko-logika</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>


</head>
<body>

<?php 
include("assets/include/header.php");
?>
<main>

    <section class="main_info">
        <div class="main_header">
            <h1 class="main_text">Programų palyginimas</h1>
            <br>
            <h4>Pasirink dvi bet kurias programas ir gausi šių programų kompiuterio resursų išnaudojimo palyginimą!</h4>
            <br>
            <div class="dropdown-container">
                <select id="mySelect" style="width: 50%">
                    <option value=""></option>
                </select>
                <select id="mySelect2" style="width: 50%">
                    <option value=""></option>
                </select>
            </div>

            <br>
            <button id="submit-button" class="btn btn-warning" disabled>Palyginti</button>

            <script>
                $(document).ready(function() {
                    var queryParams = { limit: 1500 };
                    var endpointUrl = "http://127.0.0.1:5000/get-app";

                    $.get(endpointUrl, queryParams, function(response) {
                        var options = [];
                        $.each(response, function(index, item) {
                            options.push({ id: item.process, text: item.process });
                        });

                        $('#mySelect').select2({
                            data: options,
                            tags: true,
                            placeholder: '', // removes the default "" option
                            templateResult: formatState
                        });

                        $('#mySelect2').select2({
                            data: options,
                            tags: true,
                            placeholder: '', // removes the default "" option
                            templateResult: formatState
                        });

                        // enable button when both options are selected
                        $('select').on('change', function() {
                            var selectedValue1 = $('#mySelect').val();
                            var selectedValue2 = $('#mySelect2').val();

                            if (selectedValue1 && selectedValue2) {
                                $('#submit-button').prop('disabled', false);
                            } else {
                                $('#submit-button').prop('disabled', true);
                            }
                        });

                        $(document).on('select2:open', () => {
                            document.querySelector('.select2-search__field').focus();
                        });
                    });

                    function formatState(state) {
                        if (!state.id) {
                            return state.text;
                        }
                        var $state = $(
                            '<span>' + state.text + '</span>'
                        );
                        return $state;
                    }

                    $(document).ready(function() {
                        $('#submit-button').on('click', function() {
                            var selectedValue1 = $('#mySelect').val();
                            var selectedValue2 = $('#mySelect2').val();

                            var queryParams = {
                                process1: selectedValue1,
                                process2: selectedValue2
                            };

                            var url = "compare_app-stats.php?" + $.param(queryParams);

                            // Redirect to next page
                            window.location.href = url;
                        });
                    });
                });
            </script>
        </div>
    </section>
</main>
<header class="page-header header container-fluid">
</header>

<script src="js/bootstrap.min.js"></script>

</body>

</html>