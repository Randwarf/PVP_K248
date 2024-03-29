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
            <h1 class="main_text">Populiariausios programos pagal atliktų ištestavimų skaičių</h1>
            <br>
            <h4>Nori patikrinti programą, kurios sąraše nematai? Ieškok jos čia!</h4>
            <select id="mySelect">
                <option value=""></option>
            </select>
            <button id="submit-button" class="btn btn-warning" disabled>Ieškoti</button>

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
                            width: '400px',
                            data: options,
                            tags: true,
                            placeholder: '', // removes the default "" option
                            templateResult: formatState
                        });

                        // enable button when an option is selected
                        $('#mySelect').on('change', function() {
                            var selectedValue = $(this).val();
                            if (selectedValue) {
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
                            var selectedValue = $('#mySelect').val(); // get the selected value from the dropdown list
                            $('#input-field').val(selectedValue); // set the value of the input field
                            var url = "app-stats.php?process=" + encodeURIComponent(selectedValue);

                            // Redirect to next page
                            window.location.href = url;
                        });
                    });
                });
            </script>
        </div>
    </section>

    <section class="info_cards">
        <div id="popular-container" class="row">
                <!-- Cards will be dynamically added here -->
        </div>
    </section>
      <script>
        $(document).ready(function() {
            // Define the API endpoint URL and query parameters
            var endpointUrl = "http://127.0.0.1:5000/get-app";
            var queryParams = { limit: 10 };

            // Make the API request using jQuery's $.get() method
            $.get(endpointUrl, queryParams, function(response) {
            // If the request is successful, append the data to the table
                var container = $("#popular-container");
                response.forEach(function(row) {
                    var cardDiv = $("<div>").addClass("col-sm-12 col-md-6 col-lg-6 mb-4");
                    var card = $("<div>").addClass("card h-100 popular-card").css("min-width", "0");
                    var cardBody = $("<div>").addClass("card-body d-flex flex-column justify-content-center").html("<a href='app-stats.php?process=" + row.process + "' class=\"stretched-link\"></a>");
                    var cardImage = $("<img>").attr("src", "assets/img/programs.png").addClass("about_icons");
                    var cardTitle = $("<h5>").addClass("card-title d-inline").text(row.process).css("font-size", "24px");
                    var cardText = $("<p>").addClass("card-text").text("atlikti ištestavimai: " + row.count).css("font-size", "10px").css("text-align", "right");

                    var cardHeader = $("<div>").addClass("d-flex align-items-center");
                    cardHeader.append(cardImage, cardTitle);

                    cardBody.append(cardHeader, cardText);
                    card.append(cardBody);
                    cardDiv.append(card);
                    container.append(cardDiv);
    });
  });
});

      </script>
</main>
<header class="page-header header container-fluid">
</header>

<script src="js/bootstrap.min.js"></script>

</body>

</html>