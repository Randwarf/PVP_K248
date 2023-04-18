<?php
if (session_status() !== PHP_SESSION_ACTIVE) {
    session_start();
}

if(!empty($_GET['session_id'])){ 
    $session_id = $_GET['session_id']; 

    $productName = "Ekologika - Premium narystė";  
    $productID = "EKO1553"; 
    $productPrice = 4.99;
    $currency = 'eur';
    define('STRIPE_API_KEY', 'sk_test_51MyEERL4tZqvGJSDrfI66fLAYSe0WKcWczjkDzA8sJGsGdz1T4nABTlmxWHWHU7Og8Yd7NgbpA0KMZQSfZks4crJ00wdiXS6Ry'); 
    define('STRIPE_PUBLISHABLE_KEY', 'pk_test_51MyEERL4tZqvGJSDsJK8eBdpIosYEIvGTZvmeBJs4PESKezixMg5RvMrgV7i98R6gSRO3Ov9nYM1gh6gh5kXvZTV001cF7Io74'); 
    define('STRIPE_SUCCESS_URL', 'http://localhost:3000/premium-membership/payment-success.php');
    define('STRIPE_CANCEL_URL', 'http://localhost:3000/premium-membership/payment-cancel.php');

    // Include the Stripe PHP library 
    require_once '../vendor/stripe/stripe-php/init.php'; 
        
    // Set API key 
    $stripe = new \Stripe\StripeClient(STRIPE_API_KEY); 
        
    // Fetch the Checkout Session to display the JSON result on the success page 
    try { 
        $checkout_session = $stripe->checkout->sessions->retrieve($session_id); 
    } catch(Exception $e) {  
        $api_error = $e->getMessage();  
    } 
        
    if(empty($api_error) && $checkout_session){ 
        // Get customer details 
        $customer_details = $checkout_session->customer_details; 

        // Retrieve the details of a PaymentIntent 
        try { 
            $paymentIntent = $stripe->paymentIntents->retrieve($checkout_session->payment_intent); 
        } catch (\Stripe\Exception\ApiErrorException $e) { 
            $api_error = $e->getMessage(); 
        } 
            
        if(empty($api_error) && $paymentIntent){ 
            // Check whether the payment was successful 
            if(!empty($paymentIntent) && $paymentIntent->status == 'succeeded'){ 
                // Transaction details  
                $transactionID = $paymentIntent->id; 
                $paidAmount = $paymentIntent->amount; 
                $paidAmount = ($paidAmount/100); 
                $paidCurrency = $paymentIntent->currency; 
                $payment_status = $paymentIntent->status; 
                    
                // Customer info 
                $customer_name = $customer_email = ''; 
                if(!empty($customer_details)){ 
                    $customer_name = !empty($customer_details->name)?$customer_details->name:''; 
                    $customer_email = !empty($customer_details->email)?$customer_details->email:''; 
                } 

                $url = "http://127.0.0.1:5000/make_premium?email=" . urlencode($_SESSION['USERINFO']->email);
                $options = array(
                    'http' => array(
                        'method' => 'GET',
                        'ignore_errors' => true
                    )
                );
                $context = stream_context_create($options);
                file_get_contents($url, false, $context);

                // CHANGE THIS:
                $payment_id = 1;
                    
                $status = 'success'; 
                $statusMsg = 'Your Payment has been Successful!'; 
            }else{ 
                $statusMsg = "Transaction has been failed!"; 
            } 
        }else{ 
            $statusMsg = "Unable to fetch the transaction details! $api_error";  
        } 
    }else{ 
        $statusMsg = "Invalid Transaction! $api_error";  
    }
}else{ 
    $statusMsg = "Invalid Request!"; 
} 
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>eko-logika</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="../css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="../css/style.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>

<body>
<?php if(!empty($payment_id)){ ?>
    <div class="card">
        <div class="card-header">
            <h4 class="card-title">Payment Information</h4>
        </div>
        <div class="card-body">
            <!-- <p><b>Reference Number:</b> <?php //echo $payment_id; ?></p> -->
            <p><b>Transaction ID:</b> <?php echo $transactionID; ?></p>
            <p><b>Paid Amount:</b> <?php echo $paidAmount.' '.$paidCurrency; ?></p>
            <p><b>Payment Status:</b> <?php echo $payment_status; ?></p>
        </div>
    </div>
    <div class="card">
        <div class="card-header">
            <h4 class="card-title">Customer Information</h4>
        </div>
        <div class="card-body">
            <p><b>Name:</b> <?php echo $customer_name; ?></p>
            <p><b>Email:</b> <?php echo $customer_email; ?></p>
            <p><b>Purchased For:</b> <?php echo $_SESSION['USERINFO']->email; ?></p>
        </div>
    </div>
    <div class="card">
        <div class="card-header">
            <h4 class="card-title">Product Information</h4>
        </div>
        <div class="card-body">
            <p><b>Name:</b> <?php echo $productName; ?></p>
            <p><b>Price:</b> <?php echo $productPrice.' '.$currency; ?></p>
        </div>
    </div>
<?php }else{ ?>
    <div class="card">
        <div class="card-header">
            <h4 class="card-title error">Your Payment has failed!</h4>
        </div>
        <div class="card-body">
            <p class="error"><?php echo $statusMsg; ?></p>
        </div>
    </div>
<?php } ?>

<a href="../user.php"><button>Atgal į naudotojo puslapį</button></a>
</body>
</html>