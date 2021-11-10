/// <reference types="Cypress" />
import {PartialViewBuilder} from "sbn-cypress-testhelpers";

context('Partial Views', () => {

    beforeEach(() => {
        cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
    });

    function navigateToSettings() {
        cy.sbnSection('settings');
        cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");
    }

    function openPartialViewsCreatePanel() {
        navigateToSettings();
        cy.sbnTreeItem("settings", ["Partial Views"]).rightclick();
    }

    it('Create new empty partial view', () => {
        const name = "TestPartialView";
        const fileName = name + ".cshtml";

        cy.sbnEnsurePartialViewNameNotExists(fileName);

        openPartialViewsCreatePanel();

        cy.sbnContextMenuAction("action-create").click();
        cy.get('.menu-label').first().click(); // TODO: Fucked we cant use something like cy.sbnContextMenuAction("action-mediaType").click();

        //Type name
        cy.sbnEditorHeaderName(name);

        //Save
        cy.get('.btn-success').click();

        //Assert
        cy.sbnSuccessNotification().should('be.visible');
        cy.sbnPartialViewExists(fileName).then(exists => { expect(exists).to.be.true; });

        //Clean up
        cy.sbnEnsurePartialViewNameNotExists(fileName);
    });

    it('Create partial view from snippet', () => {
        const name = "TestPartialViewFromSnippet";
        const fileName = name + ".cshtml";

        cy.sbnEnsurePartialViewNameNotExists(fileName);

        openPartialViewsCreatePanel();

        cy.sbnContextMenuAction("action-create").click();
        cy.get('.menu-label').eq(1).click();
        // Select snippet
        cy.get('.menu-label').eq(2).click();

        // Type name
        cy.sbnEditorHeaderName(name);

        // Save
        cy.get('.btn-success').click();

        // Assert
        cy.sbnSuccessNotification().should('be.visible');
        cy.sbnPartialViewExists(fileName).then(exists => { expect(exists).to.be.true; });

        // Clean up
        cy.sbnEnsurePartialViewNameNotExists(fileName);
    });

    it('Partial view with no name', () => {
        openPartialViewsCreatePanel();

        cy.sbnContextMenuAction("action-create").click();
        cy.get('.menu-label').first().click();

        // The test would fail intermittently, most likely because the editor didn't have time to load
        // This should ensure that the editor is loaded and the test should no longer fail unexpectedly.
        cy.get('.ace_content', {timeout: 5000}).should('exist');

        // Click save
        cy.get('.btn-success').click();

        // Asserts
        cy.sbnErrorNotification().should('be.visible');
    });

    it('Delete partial view', () => {
        const name = "TestDeletePartialView";
        const fileName = name + ".cshtml";

        cy.sbnEnsurePartialViewNameNotExists(fileName);

        // Build and save partial view
        const partialView = new PartialViewBuilder()
            .withName(name)
            .withContent("@inherits USbn.Cms.Web.Common.Views.SbnViewPage")
            .build();

        cy.savePartialView(partialView);

        navigateToSettings();

        // Delete partial view
        cy.sbnTreeItem("settings", ["Partial Views", fileName]).rightclick();
        cy.sbnContextMenuAction("action-delete").click();
        cy.sbnButtonByLabelKey("general_ok").click();

        // Assert
        cy.contains(fileName).should('not.exist');
        cy.sbnPartialViewExists(fileName).then(exists => { expect(exists).to.be.false; });

        // Clean
        cy.sbnEnsurePartialViewNameNotExists(fileName);
    });

    it('Edit partial view', () => {
        const name = 'EditPartialView';
        const fileName = name + ".cshtml";

        cy.sbnEnsurePartialViewNameNotExists(fileName);

        const partialView = new PartialViewBuilder()
            .withName(name)
            .withContent("@inherits Sbn.Cms.Web.Common.Views.SbnViewPage\n")
            .build();

        cy.savePartialView(partialView);

        navigateToSettings();
        // Open partial view
        cy.sbnTreeItem("settings", ["Partial Views", fileName]).click();
        // Edit
        cy.get('.ace_text-input').type("var num = 5;", {force:true} );
        cy.get('.btn-success').click();

        // Assert
        cy.sbnSuccessNotification().should('be.visible');
        // Clean
        cy.sbnEnsurePartialViewNameNotExists(fileName);
    });


});
