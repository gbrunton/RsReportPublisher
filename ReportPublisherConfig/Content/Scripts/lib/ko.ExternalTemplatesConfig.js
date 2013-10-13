$(function(){
	ko.nativeExternalTemplateEngine.instance.setOptions({
		templateUrl: "/CommonTemplates",
		templateContainerRules: {
			ContactInfoEditorHTMLTemplates: /^contactInfoEditor/,
			ContactInfoNotesEditorHTMLTemplates: /^contactInfoNotes/,
			PageLayoutTemplates: /^pageLayout/,
			ParticipantTransferHTMLTemplates: /^pTransfer/,
			ParticipantSearchHTMLTemplates: /^pSearch/,
			ReportExtensionHTMLTemplates: /^reportExtension/,
			WarningTemplates: /^warning/,
			BasicDialogTemplates: /^basicDialog/,
			ExtraInformationTemplates: /^extraInformation/,
			QuickEntryTemplates: /^quickEntry/,
			ComplexRuleHTMLTemplates: /^complexRule/,
			ReportManagerPanelTemplates: /^reportTree/
		}
	});
});