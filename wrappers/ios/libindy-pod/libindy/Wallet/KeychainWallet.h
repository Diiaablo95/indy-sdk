//
//  KeychainWallet.h
//  libindy-demo
//
//  Created by Anastasia Tarasova on 04/09/2017.
//  Copyright © 2017 Kirill Neznamov. All rights reserved.
//

#import "IndyWallet.h"

@interface KeychainWallet : NSObject <IndyWalletImplementation>

+ (KeychainWallet*) sharedInstance;

- (NSString *)walletTypeName;

@end
