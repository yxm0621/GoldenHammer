//
//  NetworkConnectivityBinding.m
//  NativePluginIOSWorkspace
//
//  Created by Ashwin kumar on 11/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NetworkConnectivityBinding.h"
#import "NetworkConnectivityHandler.h"

void initNetworkConnectivity (const char *hostURL)
{
	// Set host
	[NetworkConnectivityHandler setHostURL:ConvertToNSString(hostURL)];
	
	// Initialise
	[NetworkConnectivityHandler Instance];
}
