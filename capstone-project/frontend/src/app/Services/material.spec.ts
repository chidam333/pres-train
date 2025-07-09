import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { Material } from './material';
import { AuthFetch } from '../auth/auth-service/auth-fetch';

describe('Material Service', () => {
  let service: Material;
  let mockAuthFetch: jasmine.SpyObj<AuthFetch>;

  beforeEach(() => {
    mockAuthFetch = jasmine.createSpyObj('AuthFetch', ['getStoredToken']);
    mockAuthFetch.getStoredToken.and.returnValue('fake-token');

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        Material,
        { provide: AuthFetch, useValue: mockAuthFetch }
      ]
    });
    service = TestBed.inject(Material);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get materials by lesson ID successfully', async () => {
    const mockMaterials = { '$values': [{ id: 1, title: 'Material 1' }, { id: 2, title: 'Material 2' }] };
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockMaterials),
      { status: 200 }
    )));

    const result = await service.getMaterialsByLessonId(1);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/material/lesson/1'),
      jasmine.objectContaining({
        method: 'GET',
        headers: jasmine.objectContaining({
          'Authorization': 'Bearer fake-token'
        })
      })
    );
    expect(result).toEqual(mockMaterials['$values']);
  });

  it('should return empty array when no materials found', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify({}),
      { status: 200 }
    )));

    const result = await service.getMaterialsByLessonId(1);

    expect(result).toEqual([]);
  });

  it('should handle material fetch failure', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      'Failed to fetch materials',
      { status: 404 }
    )));

    const result = await service.getMaterialsByLessonId(1);

    expect(result).toEqual({ error: 'Failed to fetch materials' });
  });

  it('should get material by material ID successfully', async () => {
    const mockMaterial = { id: 1, title: 'Test Material', content: 'Test content' };
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify(mockMaterial),
      { status: 200 }
    )));

    const result = await service.getMaterialByMaterialId(1);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/material/1'),
      jasmine.objectContaining({
        method: 'GET'
      })
    );
    expect(result).toEqual(mockMaterial);
  });

  it('should delete material successfully', async () => {
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify({ success: true }),
      { status: 200 }
    )));

    const result = await service.deleteMaterial(1);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/material/1'),
      jasmine.objectContaining({
        method: 'DELETE'
      })
    );
    expect(result.success).toBe(true);
  });

  it('should upload material with file successfully', async () => {
    const mockFile = new File(['test content'], 'test.pdf', { type: 'application/pdf' });
    spyOn(window, 'fetch').and.returnValue(Promise.resolve(new Response(
      JSON.stringify({ id: 1, title: 'Uploaded Material' }),
      { status: 200 }
    )));

    const result = await service.uploadMaterial(1, 'Test Material', 1, mockFile);

    expect(window.fetch).toHaveBeenCalledWith(
      jasmine.stringContaining('/material'),
      jasmine.objectContaining({
        method: 'POST',
        headers: jasmine.objectContaining({
          'Authorization': 'Bearer fake-token'
        })
      })
    );
    expect(result.id).toBe(1);
  });
});
