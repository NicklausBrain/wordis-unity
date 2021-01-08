#import <Foundation/Foundation.h>

UISelectionFeedbackGenerator* SelectionFeedbackGenerator;
UINotificationFeedbackGenerator* NotificationFeedbackGenerator;
UIImpactFeedbackGenerator* LightImpactFeedbackGenerator;
UIImpactFeedbackGenerator* MediumImpactFeedbackGenerator;
UIImpactFeedbackGenerator* HeavyImpactFeedbackGenerator;

void InitHapticFeedback()
{
    SelectionFeedbackGenerator = [[UISelectionFeedbackGenerator alloc] init];
    NotificationFeedbackGenerator = [[UINotificationFeedbackGenerator alloc] init];
    LightImpactFeedbackGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
    MediumImpactFeedbackGenerator =  [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
    HeavyImpactFeedbackGenerator =  [[UIImpactFeedbackGenerator alloc] initWithStyle:
                                     UIImpactFeedbackStyleHeavy];
}

void ReleaseHapticFeedback ()
{
    SelectionFeedbackGenerator = nil;
    NotificationFeedbackGenerator = nil;
    LightImpactFeedbackGenerator = nil;
    MediumImpactFeedbackGenerator = nil;
    HeavyImpactFeedbackGenerator = nil;
}

void PrepareSelectionFeedbackGenerator()
{
    [SelectionFeedbackGenerator prepare];
}

void PrepareNotificationFeedbackGenerator()
{
    [NotificationFeedbackGenerator prepare];
}

void PrepareLightImpactFeedbackGenerator()
{
    [LightImpactFeedbackGenerator prepare];
}

void PrepareMediumImpactFeedbackGenerator()
{
    [MediumImpactFeedbackGenerator prepare];
}

void PrepareHeavyImpactFeedbackGenerator()
{
    [HeavyImpactFeedbackGenerator prepare];
}

// FEEDBACK TRIGGER METHODS -------------------------------------------------------------------------

void SelectionHaptic()
{
    [SelectionFeedbackGenerator prepare];
    [SelectionFeedbackGenerator selectionChanged];
}

void SuccessHaptic()
{
    [NotificationFeedbackGenerator prepare];
    [NotificationFeedbackGenerator notificationOccurred:UINotificationFeedbackTypeSuccess];
}

void WarningHaptic()
{
    [NotificationFeedbackGenerator prepare];
    [NotificationFeedbackGenerator notificationOccurred:UINotificationFeedbackTypeWarning];
}

void FailureHaptic()
{
    [NotificationFeedbackGenerator prepare];
    [NotificationFeedbackGenerator notificationOccurred:UINotificationFeedbackTypeError];
}

void LightImpactHaptic()
{
    [LightImpactFeedbackGenerator prepare];
    [LightImpactFeedbackGenerator impactOccurred];
}

void MediumImpactHaptic()
{
    [MediumImpactFeedbackGenerator prepare];
    [MediumImpactFeedbackGenerator impactOccurred];
}

void HeavyImpactHaptic()
{
    [HeavyImpactFeedbackGenerator prepare];
    [HeavyImpactFeedbackGenerator impactOccurred];
}
