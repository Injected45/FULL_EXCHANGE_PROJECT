<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Controllers\Controller as Controller;
use App\Enums\ResponseEnums;


class BaseController extends Controller
{
     
    /**
     * success response method.
     *
     * @return \Illuminate\Http\Response
     */
    public function sendResponse($result, $message , $code = 200 , $key = ResponseEnums::SUCCESS  )
    {
        
        $response = [
            'data'    => $result,
            'message' => $message,
            'success' => true,
            'key'=> $key
        ];

  
        return response()->json($response, $code);
    }
  
    /**
     * return error response.
     *
     * @return \Illuminate\Http\Response
     */
    public function sendError($error, $errorMessages = [], $code = 404 ,  $enum = "")
    {
        $response = [
            'success' => false,
            'message' => $error,
            'key'=> $enum,
          
        ];
  
        if(!empty($errorMessages)){
            $response['data'] = $errorMessages;
        }
  
        return response()->json($response, $code);
    }
}
