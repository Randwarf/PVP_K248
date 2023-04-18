<?php 

$productName = "Ekologika - Premium narystė";  
$productID = "EKO1553"; 
$productPrice = 4.99;
$currency = 'eur';
define('STRIPE_API_KEY', 'sk_test_51MyEERL4tZqvGJSDrfI66fLAYSe0WKcWczjkDzA8sJGsGdz1T4nABTlmxWHWHU7Og8Yd7NgbpA0KMZQSfZks4crJ00wdiXS6Ry'); 
define('STRIPE_PUBLISHABLE_KEY', 'pk_test_51MyEERL4tZqvGJSDsJK8eBdpIosYEIvGTZvmeBJs4PESKezixMg5RvMrgV7i98R6gSRO3Ov9nYM1gh6gh5kXvZTV001cF7Io74'); 
define('STRIPE_SUCCESS_URL', 'http://localhost:3000/premium-membership/payment-success.php');
define('STRIPE_CANCEL_URL', 'http://localhost:3000/premium-membership/payment-cancel.php');

// Include the configuration file 
//require_once 'config.php'; 
 
// Include the Stripe PHP library 
require_once '../vendor/stripe/stripe-php/init.php'; 
 
// Set API key 
$stripe = new \Stripe\StripeClient(STRIPE_API_KEY); 
 
$response = array( 
    'status' => 0, 
    'error' => array( 
        'message' => 'Invalid Request!'
    ) 
); 
 
if ($_SERVER['REQUEST_METHOD'] == 'POST') { 
    $input = file_get_contents('php://input'); 
    $request = json_decode($input);
}

if (json_last_error() !== JSON_ERROR_NONE) { 
    http_response_code(400); 
    echo json_encode($response); 
    exit; 
} 
 
if(!empty($request->createCheckoutSession)){ 
    // Convert product price to cent 
    $stripeAmount = round($productPrice * 100, 2); 
 
    // Create new Checkout Session for the order 
    try { 
        $checkout_session = $stripe->checkout->sessions->create([ 
            'line_items' => [[ 
                'price_data' => [ 
                    'product_data' => [ 
                        'name' => $productName, 
                        'metadata' => [ 
                            'pro_id' => $productID 
                        ] 
                    ], 
                    'unit_amount' => $stripeAmount, 
                    'currency' => $currency, 
                ], 
                'quantity' => 1 
            ]], 
            'mode' => 'payment', 
            'success_url' => STRIPE_SUCCESS_URL.'?session_id={CHECKOUT_SESSION_ID}', 
            'cancel_url' => STRIPE_CANCEL_URL, 
        ]); 
    } catch(Exception $e) {  
        $api_error = $e->getMessage();  
    } 
     
    if(empty($api_error) && $checkout_session){ 
        $response = array( 
            'status' => 1, 
            'message' => 'Checkout Session created successfully!', 
            'sessionId' => $checkout_session->id 
        ); 
    }else{ 
        $response = array( 
            'status' => 0, 
            'error' => array( 
                'message' => 'Checkout Session creation failed! '.$api_error    
            ) 
        ); 
    } 
} 
 
// Return response 
echo json_encode($response); 
?>