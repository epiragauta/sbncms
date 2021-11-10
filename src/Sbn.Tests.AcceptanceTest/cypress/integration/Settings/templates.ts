/// <reference types="Cypress" />
import {TemplateBuilder} from 'sbn-cypress-testhelpers';

context('Templates', () => {

    beforeEach(() => {
        cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
    });

    function navigateToSettings() {
        cy.sbnSection('settings');
        cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");
    }

    function createTemplate() {
        navigateToSettings();
        cy.sbnTreeItem("settings", ["Templates"]).rightclick();
        cy.sbnContextMenuAction("action-create").click();
    }

    it('Create template', () => {
        const name = "Create template test";
        cy.sbnEnsureTemplateNameNotExists(name);

        createTemplate();
        // We have to wait for the ace editor to load, because when the editor is loading it will "steal" the focus briefly,
        // which causes the save event to fire if we've added something to the header field, causing errors.
        cy.wait(500);

        //Type name
        cy.sbnEditorHeaderName(name);
        // Save
        // We must drop focus for the auto save event to occur.
        cy.get('.btn-success').focus();
        // And then wait for the auto save event to finish by finding the page in the tree view.
        // This is a bit of a roundabout way to find items in a tree view since we dont use sbnTreeItem
        // but we must be able to wait for the save event to finish, and we can't do that with sbnTreeItem
        cy.get('[data-element="tree-item-templates"] > :nth-child(2) > .umb-animated > .umb-tree-item__inner > .umb-tree-item__label')
            .contains(name).should('be.visible', { timeout: 10000 });
        // Now that the auto save event has finished we can save
        // and there wont be any duplicates or file in use errors.
        cy.get('.btn-success').click();

        //Assert
        cy.sbnSuccessNotification().should('be.visible');
        // For some reason cy.sbnErrorNotification tries to click the element which is not possible
        // if it doesn't actually exist, making should('not.be.visible') impossible.
        cy.get('.umb-notifications__notifications > .alert-error').should('not.exist');

        //Clean up
        cy.sbnEnsureTemplateNameNotExists(name);
    });

    it('Unsaved changes stay', () => {
        const name = "Templates Unsaved Changes Stay test";
        const edit = "var num = 5;";
        cy.sbnEnsureTemplateNameNotExists(name);

        const template = new TemplateBuilder()
            .withName(name)
            .withContent('@inherits Sbn.Cms.Web.Common.Views.SbnViewPage\n')
            .build();

        cy.saveTemplate(template);

        navigateToSettings();

        // Open partial view
        cy.sbnTreeItem("settings", ["Templates", name]).click();
        // Edit
        cy.get('.ace_text-input').type(edit, {force:true} );

        // Navigate away
        cy.sbnSection('content');
        // Click stay button
        cy.get('umb-button[label="Stay"] button:enabled').click();

        // Assert
        // That the same document is open
        cy.get('#headerName').should('have.value', name);
        cy.get('.ace_content').contains(edit);

        cy.sbnEnsureTemplateNameNotExists(name);
    });

    it('Discard unsaved changes', () => {
        const name = "Discard changes test";
        const edit = "var num = 5;";

        cy.sbnEnsureTemplateNameNotExists(name);

        const template = new TemplateBuilder()
            .withName(name)
            .withContent('@inherits Sbn.Cms.Web.Common.Views.SbnViewPage\n')
            .build();

        cy.saveTemplate(template);

        navigateToSettings();

        // Open partial view
        cy.sbnTreeItem("settings", ["Templates", name]).click();
        // Edit
        cy.get('.ace_text-input').type(edit, {force:true} );

        // Navigate away
        cy.sbnSection('content');
        // Click discard
        cy.get('umb-button[label="Discard changes"] button:enabled').click();
        // Navigate back
        cy.sbnSection('settings');

        // Asserts
        cy.get('.ace_content').should('not.contain', edit);
        // cy.sbnPartialViewExists(fileName).then(exists => { expect(exists).to.be.false; }); TODO: Switch to template
        cy.sbnEnsureTemplateNameNotExists(name);
    });

    it('Insert macro', () => {
        const name = 'InsertMacroTest';

        cy.sbnEnsureTemplateNameNotExists(name);
        cy.sbnEnsureMacroNameNotExists(name);

        const template = new TemplateBuilder()
            .withName(name)
            .withContent('')
            .build();

        cy.saveTemplate(template);

        cy.saveMacro(name);

        navigateToSettings();
        cy.sbnTreeItem("settings", ["Templates", name]).click();
        // Insert macro
        cy.sbnButtonByLabelKey('general_insert').click();
        cy.get('.umb-insert-code-box__title').contains('Macro').click();
        cy.get('.umb-card-grid-item').contains(name).click();

        // Assert
        cy.get('.ace_content').contains('@await Sbn.RenderMacroAsync("' + name + '")').should('exist');

        // Clean
        cy.sbnEnsureTemplateNameNotExists(name);
        cy.sbnEnsureMacroNameNotExists(name);
    });

    it('Insert value', () => {
        const name = 'Insert Value Test';

        cy.sbnEnsureTemplateNameNotExists(name);

        const partialView = new TemplateBuilder()
            .withName(name)
            .withContent('')
            .build();

        cy.saveTemplate(partialView);

        navigateToSettings();
        cy.sbnTreeItem("settings", ["Templates", name]).click();

        // Insert value
        cy.sbnButtonByLabelKey('general_insert').click();
        cy.get('.umb-insert-code-box__title').contains('Value').click();
        cy.get('select').select('sbnBytes');
        cy.sbnButtonByLabelKey('general_submit').click();

        // assert
        cy.get('.ace_content').contains('@Model.Value("sbnBytes")').should('exist');

        // Clean
        cy.sbnEnsureTemplateNameNotExists(name);
    });

});
