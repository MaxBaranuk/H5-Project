#import "UnityAppController.h"

@interface iOSNativeShare : UIViewController
{
    UINavigationController *navController;
}


struct ConfigStruct {
    char* title;
    char* message;
};

struct SocialSharingStruct {
    char* text;
    char* url;
    char* image;
    char* subject;
};


#ifdef __cplusplus
extern "C" {
#endif
    
    void _makeToast();
    void showSocialSharing(struct SocialSharingStruct *confStruct);
	void sendWhatsappMessage();
    
#ifdef __cplusplus
}
#endif


@end