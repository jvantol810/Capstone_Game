<?php

    $con = mysqli_connect('localhost', 'root', 'root', 'csc483capstone');
    //Check that the connect was established.
    if (mysqli_connect_errno())
    {
        echo "1: Failed to connect."; // Error code 1 = connection failed.
        exit();
    }
    //Connection was established.
    
	//$_post['username'] = 'JohnGoodlaski';
	//$_post['password'] = 'Password';
	
	$playerid = $_POST['PLAYERID'];
	$currency = $_POST['CURRENCY'];

	if(empty($playerid) || empty($currency))
	{
		echo "Did not receive playerid or currency.";
		exit();
	}

	$updatequery = "UPDATE accounts SET currency = " . $currency . " WHERE playerid = $playerid;";
		$updatecheck = mysqli_query($con, $updatequery) or die("7: Failed to update currency" . mysqli_error($con));

		echo "Successfully Updated Currency";


	/*
	if(empty($username))
	{
		echo "Name is empty";
	}
	else
	{	
		
		//Check if name exists
		$namecheckquery = "SELECT playerid, username, salt, hash from accounts WHERE username='" . $username . "';";

		$namecheck = mysqli_query($con,$namecheckquery) or die("2: Namecheck Query Failed" . mysqli_error($con));
		

		if (mysqli_num_rows($namecheck) != 1)
		{
			echo "3: Username not found"; //error #3
			exit();
		}


		$salt = "";
		$hash = "";
		$playerid = "";
			//get login info from query
		while($row = mysqli_fetch_array($namecheck))
		{
			//$output = $output . "/t" . $row['username'] . "/t" . $row['time'];
			$salt = $row["salt"];
			$hash = $row["hash"];
			$playerid = $row["playerid"];
		}

		
		//get login info from query
		//$tableinfo = mysqli_fetch_assoc($namecheck);
		
		
		$loginhash = crypt($password, $salt);
		if ($hash != $loginhash)
		{
			echo "4: Incorrect Password"; //Error #4
			exit();
		}
		
		

	}*/

   
?>