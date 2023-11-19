<?php
if (isset($_REQUEST['firstname'],$_REQUEST['email'])) {
      
    $firstname = $_REQUEST['firstname'];
	$lastname = $_REQUEST['lastname'];
    $email = $_REQUEST['email'];
    $message = $_REQUEST['message'];
      
    // Set your email address where you want to receive emails. 
    $to = 'youremail@mail.com';
      
    $subject = 'Contact Request From Website';
    $headers = "From: ".$firstname." <".$email."> \r\n";
      
    $send_email = mail($to,$subject,$message,$headers);
      
    echo ($send_email) ? 'success' : 'error';
      
}
?>