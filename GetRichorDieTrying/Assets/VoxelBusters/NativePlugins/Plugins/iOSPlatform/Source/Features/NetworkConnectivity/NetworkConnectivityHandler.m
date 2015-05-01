//
//  NetworkConnectivityHandler.m
//  Unity-iPhone
//
//  Created by Ashwin kumar on 06/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NetworkConnectivityHandler.h"

static NSString *HostURLAddress;

@implementation NetworkConnectivityHandler

#define kNetworkChanged		"ConnectivityChanged"

@synthesize isHostReachable;
@synthesize isWifiReachable;
@synthesize isConnected;
@synthesize hostReachability;
@synthesize wifiReachability;

+ (void)setHostURL:(NSString *)hostURL
{
	NSString *oldURL	= HostURLAddress;
	
	// Assign new value
	HostURLAddress		= [hostURL retain];
	
	// Release old value
	if (oldURL)
		[oldURL release];
}

#pragma mark - init and dealloc

- (id)init
{
	self = [super init];
    
    if (self)
    {
        // Default flag values
        self.isHostReachable    = NO;
        self.isWifiReachable 	= NO;
        self.isConnected		= NO;
        
        // Register for notification
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(reachabilityChanged:)
                                                     name:kReachabilityChangedNotification
                                                   object:nil];
        
        // Initialise reachablity check for internet and wifi
        self.hostReachability   = [Reachability reachabilityWithHostName:HostURLAddress];
        [self.hostReachability startNotifier];
        [self updateInterfaceWithReachability:self.hostReachability];
        
        self.wifiReachability	= [Reachability reachabilityForLocalWiFi];
        [self.wifiReachability startNotifier];
        [self updateInterfaceWithReachability:self.wifiReachability];
    }
    
    return self;
}

- (void)dealloc
{
    // Remove as observer
    [[NSNotificationCenter defaultCenter] removeObserver:self
                                                    name:kReachabilityChangedNotification
                                                  object:nil];
	if (HostURLAddress != NULL)
		[HostURLAddress	release], HostURLAddress = NULL;
	
    // Release
    self.hostReachability   = NULL;
    self.wifiReachability	= NULL;
    
    [super dealloc];
}

#pragma mark - Notification

- (void)reachabilityChanged:(NSNotification *)note
{
	Reachability* curReach = [note object];
    
    if ([curReach isKindOfClass:[Reachability class]])
    {
        [self updateInterfaceWithReachability: curReach];
    }
}

- (void)updateInterfaceWithReachability:(Reachability *)curReach
{
    if (curReach == self.hostReachability)
	{
		self.isHostReachable    = [self checkIfReachable:curReach];
	}
	else if (curReach == self.wifiReachability)
	{
		self.isWifiReachable 	= [self checkIfReachable:curReach];
	}
    
    // Check if reachable or not
    bool newConnectivityStatus	= self.isHostReachable || self.isWifiReachable;
    
    if (self.isConnected)
    {
        if (!newConnectivityStatus)
        {
            NSLog(@"[NetworkConnectivityHandler] is not reachable");
            NotifyEventListener(kNetworkChanged, "false");
        }
    }
    else
    {
        if (newConnectivityStatus)
        {
            NSLog(@"[NetworkConnectivityHandler] is reachable");
            NotifyEventListener(kNetworkChanged, "true");
        }
    }
	
	// Update flag
	self.isConnected	= newConnectivityStatus;
}

- (bool)checkIfReachable:(Reachability *)curReach
{
    NetworkStatus netStatus = [curReach currentReachabilityStatus];
    
	return ((netStatus != NotReachable)? true : false);
}

@end