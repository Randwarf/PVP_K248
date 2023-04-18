<?php
    if (session_status() !== PHP_SESSION_ACTIVE) {
        session_start();
    }

    if (!isset($_SESSION['USERINFO'])) {
        header("Location: index.php");
    }

    define('STRIPE_API_KEY', 'sk_test_51MyEERL4tZqvGJSDrfI66fLAYSe0WKcWczjkDzA8sJGsGdz1T4nABTlmxWHWHU7Og8Yd7NgbpA0KMZQSfZks4crJ00wdiXS6Ry'); 
    define('STRIPE_PUBLISHABLE_KEY', 'pk_test_51MyEERL4tZqvGJSDsJK8eBdpIosYEIvGTZvmeBJs4PESKezixMg5RvMrgV7i98R6gSRO3Ov9nYM1gh6gh5kXvZTV001cF7Io74'); 
    define('STRIPE_SUCCESS_URL', 'http://localhost:3000/premium-membership/payment-success.php');
    define('STRIPE_CANCEL_URL', 'http://localhost:3000/premium-membership/payment-cancel.php');
    //include('premium-membership/config.php');
    $productName = "Ekologika - Premium narystė";  
    $productID = "EKO1553"; 
    $productPrice = 12.99;
    $currency = 'eur';

    $url = "http://127.0.0.1:5000/get-user-byemail?email=" . urlencode($_SESSION['USERINFO']->email);
    $options = array(
        'http' => array(
            'method' => 'GET',
            'ignore_errors' => true
        )
    );
    $context = stream_context_create($options);
    $response = file_get_contents($url, false, $context);
    $response_json = json_decode($response, true);
    $premiumEndDate = $response_json['premiumEndDate'];

    $date = new DateTime($premiumEndDate);
    $today = new DateTime();
    $isPremium = $date > $today;
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>eko-logika</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://js.stripe.com/v3/"></script>
</head>
<body>

<script async src="assets/scripts/header.js"></script>

<?php 
    include("assets/include/header.php");
?>
    
<main>
    <section class="main_info">
        <div class="main_header">
                <h1 class="main_text">Naudotojas</h1>
        </div>
    </section>

    <section class="main_header">
        <div class="container">
            <h1 class="main_text" id="username"><?php echo ($_SESSION['USERINFO']->email) ?></h1>
            <br></br>
            <p>
                <?php
                echo '<b>Premium Narystė' . ($isPremium ? ' - Aktyvuota' : '') . '</b>';
                
                if ($isPremium == false) {
                    echo '<p>Kaina: <b>'.$productPrice. ' ' .strtoupper($currency) .'</b></p>';

                    echo '<button class="stripe-button btn btn-warning" id="payButton">';
                    echo '<div class="spinner hidden" id="spinner"></div>';
                    echo '<span id="buttonText">Pirkti</span>';
                    echo '</button>';
                }

                echo '<div id="paymentResponse" class="hidden"></div>';
                ?>
            </p>
        </div>
    </section>
</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>

<script>
// Set Stripe publishable key to initialize Stripe.js
const stripe = Stripe('<?php echo STRIPE_PUBLISHABLE_KEY; ?>');

// Select payment button
const payBtn = document.querySelector("#payButton");

// Payment request handler
payBtn.addEventListener("click", function (evt) {
    console.log("Hello");
    setLoading(true);

    createCheckoutSession().then(function (data) {
        if(data.sessionId){
            stripe.redirectToCheckout({
                sessionId: data.sessionId,
            }).then(handleResult);
        }else{
            handleResult(data);
        }
    });
});
    
// Create a Checkout Session with the selected product
const createCheckoutSession = function (stripe) {
    return fetch("premium-membership/payment_init.php", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            createCheckoutSession: 1,
        }),
    }).then(function (result) {
        return result.json();
    });
};

// Handle any errors returned from Checkout
const handleResult = function (result) {
    if (result.error) {
        showMessage(result.error.message);
    }
    
    setLoading(false);
};

// Show a spinner on payment processing
function setLoading(isLoading) {
    if (isLoading) {
        // Disable the button and show a spinner
        payBtn.disabled = true;
        document.querySelector("#spinner").classList.remove("hidden");
        document.querySelector("#buttonText").classList.add("hidden");
    } else {
        // Enable the button and hide spinner
        payBtn.disabled = false;
        document.querySelector("#spinner").classList.add("hidden");
        document.querySelector("#buttonText").classList.remove("hidden");
    }
}

// Display message
function showMessage(messageText) {
    const messageContainer = document.querySelector("#paymentResponse");
	
    messageContainer.classList.remove("hidden");
    messageContainer.textContent = messageText;
	
    setTimeout(function () {
        messageContainer.classList.add("hidden");
        messageText.textContent = "";
    }, 5000);
}
</script>